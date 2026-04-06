import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

import '../../../core/theme/app_colors.dart';
import '../../../core/theme/app_text_styles.dart';

/// Single hidden TextField approach — one TextField captures all digits,
/// display boxes show individual characters. Keyboard stays stable because
/// there is only ONE focus node and ONE TextField.
class OtpInputField extends StatefulWidget {
  const OtpInputField({
    super.key,
    this.length = 4,
    this.onCompleted,
    this.onChanged,
  });

  final int length;
  final ValueChanged<String>? onCompleted;
  final ValueChanged<String>? onChanged;

  @override
  State<OtpInputField> createState() => OtpInputFieldState();
}

class OtpInputFieldState extends State<OtpInputField> {
  late TextEditingController _controller;
  late FocusNode _focusNode;

  @override
  void initState() {
    super.initState();
    _controller = TextEditingController();
    _focusNode = FocusNode();
    _controller.addListener(_onTextChanged);
  }

  @override
  void dispose() {
    _controller.removeListener(_onTextChanged);
    _controller.dispose();
    _focusNode.dispose();
    super.dispose();
  }

  String get otp => _controller.text;

  void clear() {
    _controller.clear();
    _focusNode.requestFocus();
  }

  void _onTextChanged() {
    setState(() {});
    widget.onChanged?.call(_controller.text);
    if (_controller.text.length == widget.length) {
      _focusNode.unfocus();
      widget.onCompleted?.call(_controller.text);
    }
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        _focusNode.requestFocus();
      },
      child: Stack(
        children: [
          // Hidden TextField that captures input
          Opacity(
            opacity: 0,
            child: SizedBox(
              height: 48,
              child: TextField(
                controller: _controller,
                focusNode: _focusNode,
                autofocus: false,
                keyboardType: TextInputType.number,
                maxLength: widget.length,
                decoration: const InputDecoration(
                  counterText: '',
                  border: InputBorder.none,
                ),
                inputFormatters: [FilteringTextInputFormatter.digitsOnly],
              ),
            ),
          ),
          // Visible OTP boxes
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: List.generate(widget.length, (index) {
              final isFirst = index == 0;
              final text = _controller.text;
              final hasChar = index < text.length;
              final isCurrent = index == text.length && _focusNode.hasFocus;
              return Expanded(
                child: Container(
                  height: 48,
                  margin: EdgeInsets.only(left: isFirst ? 0 : 8),
                  decoration: BoxDecoration(
                    border: Border.all(
                      color: hasChar || isCurrent
                          ? AppColors.primary
                          : AppColors.gray,
                      width: isCurrent ? 2 : 1,
                    ),
                    borderRadius: BorderRadius.circular(10),
                  ),
                  alignment: Alignment.center,
                  child: Text(
                    hasChar ? text[index] : '',
                    style: const TextStyle(
                      fontFamily: AppTextStyles.fontFamily,
                      fontSize: 20,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textPrimary,
                    ),
                  ),
                ),
              );
            }),
          ),
        ],
      ),
    );
  }
}