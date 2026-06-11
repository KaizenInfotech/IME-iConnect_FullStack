import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_text_styles.dart';
import '../../../core/widgets/common_app_bar.dart';
import '../../../core/widgets/empty_state_widget.dart';
import '../providers/district_provider.dart';

/// Port of iOS district committee screen.
/// API: District/GetDistrictCommittee (POST) — reads the curated bod_master for the group.
/// Self-fetches on mount and supports pull-to-refresh so the list always reflects the portal.
class DistrictCommitteeScreen extends StatefulWidget {
  const DistrictCommitteeScreen({
    super.key,
    required this.groupId,
    this.committees = const [],
  });

  final String groupId;

  /// Optional pre-fetched seed; the screen still refreshes from the API on mount.
  final List<Map<String, dynamic>> committees;

  @override
  State<DistrictCommitteeScreen> createState() =>
      _DistrictCommitteeScreenState();
}

class _DistrictCommitteeScreenState extends State<DistrictCommitteeScreen> {
  List<Map<String, dynamic>> _committees = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _committees = widget.committees;
    WidgetsBinding.instance.addPostFrameCallback((_) => _load());
  }

  Future<void> _load() async {
    if (!mounted) return;
    setState(() => _isLoading = _committees.isEmpty);
    final list = await context
        .read<DistrictProvider>()
        .fetchDistrictCommittee(groupId: widget.groupId);
    if (!mounted) return;
    setState(() {
      _committees = list;
      _isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.scaffoldBackground,
      appBar: CommonAppBar(title: 'District Committee'),
      body: RefreshIndicator(
        onRefresh: _load,
        child: (_isLoading && _committees.isEmpty)
            ? const Center(child: CircularProgressIndicator())
            : _committees.isEmpty
                ? ListView(
                    physics: const AlwaysScrollableScrollPhysics(),
                    children: const [
                      SizedBox(height: 120),
                      EmptyStateWidget(
                        icon: Icons.groups,
                        message: 'No committee data available',
                      ),
                    ],
                  )
                : ListView.builder(
                    physics: const AlwaysScrollableScrollPhysics(),
                    itemCount: _committees.length,
                    itemBuilder: (_, index) {
                      final item = _committees[index];
                      return Container(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 16, vertical: 14),
                        decoration: const BoxDecoration(
                          color: AppColors.white,
                          border: Border(
                            bottom: BorderSide(
                                color: AppColors.divider, width: 0.5),
                          ),
                        ),
                        child: Row(
                          children: [
                            Container(
                              width: 40,
                              height: 40,
                              decoration: BoxDecoration(
                                color:
                                    AppColors.primary.withValues(alpha: 0.1),
                                shape: BoxShape.circle,
                              ),
                              child: const Icon(Icons.person,
                                  color: AppColors.primary, size: 22),
                            ),
                            const SizedBox(width: 12),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    item['name']?.toString() ?? 'Unknown',
                                    style: AppTextStyles.body2.copyWith(
                                      fontWeight: FontWeight.w500,
                                    ),
                                  ),
                                  if (item['designation'] != null) ...[
                                    const SizedBox(height: 2),
                                    Text(
                                      item['designation'].toString(),
                                      style: AppTextStyles.caption,
                                    ),
                                  ],
                                ],
                              ),
                            ),
                          ],
                        ),
                      );
                    },
                  ),
      ),
    );
  }
}
