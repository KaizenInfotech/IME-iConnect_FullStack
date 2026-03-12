-- ============================================================
-- TouchBase Data Migration - Generated from Live API Responses
-- Generated: 2026-03-09 18:10:14
-- ============================================================

USE `touchbase_db`;
SET FOREIGN_KEY_CHECKS = 0;
SET sql_mode = '';
SET @now = NOW(6);

-- ===================== COUNTRIES =====================
TRUNCATE TABLE `countries`;
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('4', '+93', 'Afghanistan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('5', '+358', 'Aland Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('6', '+355', 'Albania');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('7', '+213', 'Algeria');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('8', '+1', 'American Samoa');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('9', '+376', 'Andorra');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('10', '+244', 'Angola');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('11', '+1', 'Anguilla');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('12', '+672', 'Antarctica');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('13', '+1', 'Antigua and Barbuda');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('14', '+54', 'Argentina');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('15', '+374', 'Armenia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('16', '+297', 'Aruba');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('17', '+61', 'Australia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('18', '+43', 'Austria');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('19', '+994', 'Azerbaijan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('20', '+1', 'Bahamas');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('21', '+973', 'Bahrain');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('22', '+880', 'Bangladesh');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('23', '+1', 'Barbados');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('24', '+375', 'Belarus');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('25', '+32', 'Belgium');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('26', '+501', 'Belize');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('27', '+229', 'Benin');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('28', '+1', 'Bermuda');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('29', '+975', 'Bhutan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('30', '+591', 'Bolivia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('31', '+599', 'Bonaire');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('32', '+387', 'Bosnia and Herzegovina');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('33', '+267', 'Botswana');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('34', '+55', 'Brazil');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('35', '+1', 'British Virgin Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('36', '+673', 'Brunei Darussalam');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('37', '+359', 'Bulgaria');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('38', '+226', 'Burkina Faso');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('39', '+257', 'Burundi');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('40', '+855', 'Cambodia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('41', '+237', 'Cameroon');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('42', '+1', 'Canada');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('43', '+238', 'Cape Verde');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('44', '+1', 'Cayman Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('45', '+236', 'Central African Republic');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('46', '+235', 'Chad');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('47', '+56', 'Chile');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('48', '+86', 'China');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('49', '+57', 'Colombia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('50', '+269', 'Comoros');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('51', '+242', 'Congo');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('53', '+682', 'Cook Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('54', '+506', 'Costa Rica');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('55', '+225', 'Cote d`Ivoire');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('56', '+385', 'Croatia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('57', '+599', 'Curaccao');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('58', '+357', 'Cyprus');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('59', '+420', 'Czech Republic');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('225', '+243', 'Democratic Republic of the Congo');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('60', '+45', 'Denmark');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('61', '+253', 'Djibouti');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('62', '+1', 'Dominica');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('63', '+1', 'Dominican Republic');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('220', NULL, 'East Africa');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('64', '+1', 'EClub');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('65', '+593', 'Ecuador');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('66', '+20', 'Egypt');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('67', '+503', 'El Salvador');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('69', '+240', 'Equatorial Guinea');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('70', '+291', 'Eritrea');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('71', '+372', 'Estonia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('72', '+251', 'Ethiopia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('73', '+298', 'Faroe Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('74', '+679', 'Fiji');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('75', '+358', 'Finland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('76', '+33', 'France');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('77', '+594', 'French Guiana');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('78', '+689', 'French Polynesia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('79', '+241', 'Gabon');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('80', '+220', 'Gambia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('81', '+995', 'Georgia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('82', '+49', 'Germany');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('83', '+233', 'Ghana');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('84', '+350', 'Gibraltar');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('85', '+30', 'Greece');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('86', '+299', 'Greenland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('87', '+1', 'Grenada');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('88', '+590', 'Guadeloupe');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('89', '+1', 'Guam');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('90', '+502', 'Guatemala');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('91', '+44', 'Guernsey-Channel Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('92', '+224', 'Guinea');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('93', '+245', 'Guinea-Bissau');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('94', '+592', 'Guyana');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('95', '+509', 'Haiti');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('96', '+504', 'Honduras');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('97', '+852', 'Hong Kong');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('98', '+36', 'Hungary');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('99', '+354', 'Iceland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('1', '+91', 'India');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('100', '+62', 'Indonesia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('101', '+353', 'Ireland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('102', '+44', 'Isle of Man');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('103', '+972', 'Israel');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('104', '+39', 'Italy');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('105', '+1', 'Jamaica');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('106', '+81', 'Japan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('107', '+1', 'Jersey-Channel Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('108', '+962', 'Jordan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('109', '+7', 'Kazakhstan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('110', '+254', 'Kenya');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('111', '+686', 'Kiribati');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('112', '+850', 'Korea');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('113', '+383', 'Kosovo');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('114', '+996', 'Kyrgyzstan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('115', '+856', 'Lao People`s Democratic Republic');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('116', '+371', 'Latvia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('117', '+961', 'Lebanon');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('118', '+266', 'Lesotho');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('119', '+231', 'Liberia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('120', '+423', 'Liechtenstein');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('121', '+370', 'Lithuania');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('2', '+352', 'Luxembourg');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('122', '+853', 'Macao');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('123', '+389', 'Macedonia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('124', '+261', 'Madagascar');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('125', '+265', 'Malawi');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('126', '+60', 'Malaysia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('127', '+960', 'Maldives');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('128', '+223', 'Mali');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('129', '+356', 'Malta');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('130', '+596', 'Martinique');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('131', '+222', 'Mauritania');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('132', '+230', 'Mauritius');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('133', '+262', 'Mayotte');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('134', '+52', 'Mexico');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('135', '+691', 'Micronesia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('136', '+373', 'Moldova');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('137', '+377', 'Monaco');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('138', '+976', 'Mongolia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('139', '+382', 'Montenegro');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('140', '+1', 'Montserrat');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('141', '+212', 'Morocco');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('142', '+258', 'Mozambique');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('143', '+264', 'Namibia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('144', '+674', 'Nauru');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('145', '+977', 'Nepal');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('146', '+31', 'Netherlands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('147', '+687', 'New Caledonia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('148', '+64', 'New Zealand');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('149', '+505', 'Nicaragua');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('150', '+227', 'Niger');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('151', '+234', 'Nigeria');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('152', '+672', 'Norfolk Island');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('153', '+44', 'Northern Ireland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('154', '+1', 'Northern Mariana Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('155', '+47', 'Norway');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('222', '+968', 'Oman');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('156', '+92', 'Pakistan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('157', '+680', 'Palau');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('158', '+970', 'Palestine');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('159', '+507', 'Panama');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('160', '+675', 'Papua New Guinea');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('161', '+595', 'Paraguay');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('162', '+51', 'Peru');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('163', '+63', 'Philippines');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('164', '+48', 'Poland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('165', '+351', 'Portugal');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('166', '+1', 'Puerto Rico');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('223', '+974', 'Qatar');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('167', '+262', 'Reunion');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('168', '+40', 'Romania');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('169', '+7', 'Russian Federation');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('170', '+250', 'Rwanda');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('171', '+590', 'Saint Barthelemy');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('172', '+1', 'Saint Kitts and Nevis');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('173', '+1', 'Saint Lucia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('174', '+1', 'Saint Martin');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('175', '+508', 'Saint Pierre and Miquelon');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('176', '+1', 'Saint Vincent and the Grenadines');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('177', '+685', 'Samoa');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('178', '+378', 'San Marino');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('179', '+239', 'Sao Tome and Principe');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('224', '+966', 'Saudi Arabia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('180', '+44', 'Scotland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('181', '+221', 'Senegal');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('182', '+381', 'Serbia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('183', '+248', 'Seychelles');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('184', '+232', 'Sierra Leone');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('221', '+65', 'Singapore');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('186', '+1', 'Sint Maarten');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('187', '+421', 'Slovakia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('188', '+386', 'Slovenia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('189', '+677', 'Solomon Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('190', '+27', 'South Africa');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('226', '82', 'South Korea');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('191', '+211', 'South Sudan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('192', '+34', 'Spain');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('193', '+94', 'Sri Lanka');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('194', '+249', 'Sudan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('195', '+597', 'Suriname');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('196', '+268', 'Swaziland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('197', '+46', 'Sweden');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('198', '+41', 'Switzerland');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('199', '+886', 'Taiwan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('200', '+992', 'Tajikistan');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('201', '+255', 'Tanzania');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('202', '+66', 'Thailand');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('203', '+670', 'Timor-Leste');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('204', '+228', 'Togo');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('205', '+676', 'Tonga');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('206', '+1', 'Trinidad and Tobago');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('207', '+216', 'Tunisia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('208', '+90', 'Turkey');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('209', '+1', 'Turks and Caicos Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('210', '+256', 'Uganda');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('211', '+380', 'Ukraine');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('212', '+971', 'United Arab Emirates');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('68', '+44', 'United Kingdom');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('3', '+1', 'United States');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('213', '+1', 'United States Virgin Islands');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('214', '+598', 'Uruguay');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('215', '+678', 'Vanuatu');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('216', '+58', 'Venezuela');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('217', '+44', 'Wales');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('218', '+260', 'Zambia');
INSERT INTO `countries` (`Id`, `CountryCode`, `CountryName`) VALUES ('219', '+263', 'Zimbabwe');
-- 224 countries inserted

-- ===================== CATEGORIES =====================
TRUNCATE TABLE `categories`;
INSERT INTO `categories` (`Id`, `CatName`) VALUES (1, 'Ashore - Retired');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (2, 'Indian_Navy - Lieutenant Commander');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (3, 'Ashore - Others');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (4, 'Onboard_Ship - Chief Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (5, 'Ashore - Manager / Director- Bunkering or fuel survey');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (6, 'Ashore - Engineering or Management Consultant');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (7, 'Ashore - Classification Society');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (8, 'Ashore - Technical Superintendent / Fleet Technical Manager / Director Technical');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (9, 'Ashore - Director / CEO/ Owner');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (10, 'Onboard_Ship - Second Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (11, 'Indian_Navy - Commander');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (12, 'Ashore - Ship Related Services');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (13, 'Indian_Navy -');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (14, 'Onboard_Ship - Junior Engineer / TME / Engine cadet');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (15, 'Indian_Navy - Sub Lieutenant');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (16, 'Indian_Navy - Captain');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (17, 'Indian_Navy - Rear Admiral');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (18, 'Indian_Navy - Commodore');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (19, 'Indian_Navy - Others');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (20, 'Ashore - Naval Architect');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (21, 'Onboard_Ship - Fourth Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (22, 'Coast_Guard -');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (23, 'Ashore - Fourth Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (24, 'Ashore - Junior Engineer / TME / Engine cadet');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (25, 'Ashore - Chief Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (26, 'Indian_Navy - Lieutenant');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (27, 'Onboard_Ship - Lieutenant Commander');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (28, 'Onboard_Ship - Third Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (29, 'Onboard_Ship -');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (30, 'Onboard_Ship - Captain');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (31, 'Onboard_Ship - Rear Admiral');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (32, 'Onboard_Ship - Commander');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (33, 'Onboard_Ship - Manager / Director- Bunkering or fuel survey');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (34, 'Indian_Navy - Director / CEO/ Owner');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (35, 'Onboard_Ship - OTHERS');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (36, 'Onboard_Ship - Ship Related Services');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (37, 'Onboard_Ship - Technical Superintendent / Fleet Technical Manager / Director Technical');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (38, 'Ashore - Third Engineer');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (39, 'Onboard_Ship - Lieutenant');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (40, 'Indian_Navy - Admiral');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (41, 'Maritime_Training_Institute - Faculty / Course incharge / Professor');
INSERT INTO `categories` (`Id`, `CatName`) VALUES (42, 'Onboard_Ship - Electrical officer');
-- 42 categories inserted

-- ===================== DISTRICTS =====================
TRUNCATE TABLE `districts`;
INSERT INTO `districts` (`Id`, `DistrictName`, `DistrictNumber`) VALUES (1, 'National Admin', '31185');

-- ===================== ZONES =====================
TRUNCATE TABLE `zones`;
-- No zones data available from API

-- ===================== USERS =====================
TRUNCATE TABLE `users`;
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('501', '9875344077', '91', 'Sadhan', 'Kumar', 'Sarkar', 'sksarkarmariner@gmail.com', 'https://imeiconnect.com/Documents/directory/SK-Sarkar02012026023359PM.jpeg', '1238', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('520', '9390689530', '91', 'Vijayananda', 'Kumar', 'Amara', 'amaravk1954@gmail.com', NULL, '1283', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('574', '9815538175', '91', 'Sanjeet', NULL, 'Singh', 'sanjeet332@yahoo.co.in', NULL, '1440', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('938', '8879004806', '91', 'Chitta', 'Ranjan', 'Dash', 'chittadash1958@gmail.com', 'https://imeiconnect.com/Documents/directory/chitta-ranjan-dash02012026042738PM.jpg', '2375', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1101', '9810363384', '91', 'Atul Mani', NULL, 'Sharma', 'atulmani.sharma@gmail.com', NULL, '2723', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1204', '9848199565', '91', 'Voona', 'Lakshmipati', 'Rao', 'voonalrao@gmail.com', NULL, '2963', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1328', '9535248730', '91', 'Preetam', 'Kumar', 'Seetharam', 'preetamkumars@gmail.com', NULL, '3285', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1337', '8879366414', '91', 'Sanjeev', 'Kootu Ngal', 'Krishnapillai', 'kksanjeev19@gmail.com', NULL, '3313', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1359', '9822409657', '91', 'Sanjeev', 'Dinanath', 'Ogale', 'sanjeev@torinomail.com', NULL, '3346', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1430', '9823895454', '91', 'Deepak', 'Vaman', 'Saranjame', 'dsaranjame@gmail.com', NULL, '3548', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1564', '9814434912', '91', 'Iqbal', NULL, 'Singh', 'iqbalsingh363@yahoo.co.in', 'https://imeiconnect.com/Documents/directory/Iqbal_Singh_-_Chandigarh_Chapter02012026102353AM.jpg', '3866', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1633', '9840134947', '91', 'Ramasamy', NULL, 'Muthusamy', 'rsamy501@gmail.com', 'https://imeiconnect.com/Documents/directory/R._Muthusamy06112025022457PM.jpg', '3989', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1927', '9381013180', '91', 'Ramesh', NULL, 'Subramanian', 'chandri.ramesh@gmail.com', 'https://imeiconnect.com/Documents/directory/._Ramesh_Subramanian-_Chennai02012026104327AM.jpg', '4498', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('1961', '9820432899', '91', 'Sanjeev', 'V.', 'Mehra', 'sanjeev.mehra64@gmail.com', 'https://imeiconnect.com/Documents/directory/Sanjeev_Mehra_-_Mumbai06112025013005PM.jpeg', '4562', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('2027', '9037477904', '91', 'Dipak', NULL, 'Mohan', 'dipakmohan@gmail.com', 'https://imeiconnect.com/Documents/directory/Dipak-Mohan_Kochi02012026021922PM.jpg', '4658', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('2104', '9423160593', '91', 'Mohan', 'Ganesh', 'Joshi', 'mogajoshi@gmail.com', NULL, '4775', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('2265', '9895164498', '91', 'Sajan P.', NULL, 'John', 'sajanp.john@yahoo.co.in', NULL, '5092', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('2481', '9619547597', '91', 'Mohammed', 'Tarique', 'Mulla', 'tarique_mulla@yahoo.co.in', 'https://imeiconnect.com/Documents/directory/Tarique_Mulla_-_Navi_Mumbai02012026042829PM.png', '5712', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('3685', '9994968456', '91', 'Dr Sivasami', NULL, 'K', 'ksivasami@imu.ac.in', NULL, '12482', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('7870', '8169262115', '91', 'Jyoti', 'Kumari', 'Nayak', 'seabird_here@yahoo.com', NULL, '22979', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('8357', '9920062295', '91', 'Archana', 'Saxena', 'Sangal', 'archana_sangal@yahoo.com', 'https://imeiconnect.com/Documents/directory/Archana_Saxena_Sangal_-_Navi_Mumbai02012026042953PM.jpg', '24734', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('8462', '9440413844', '91', 'Varaha', 'Siva Prasad', 'Vanthala', 'prasadau@gmail.com', NULL, '24940', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('8572', '9488035126', '91', 'Dinesh Karumannan', NULL, 'Ramasamy', 'ram_krdinesh@yahoo.com', NULL, '25882', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('8706', '9028470600', '91', 'Ajit', 'Raghunath', 'Shelar', 'ajitshelar367@gmail.com', NULL, '27049', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('8835', '9860996837', '91', 'Sonali', NULL, 'Banerjee', 'sonali4843@gmail.com', 'https://imeiconnect.com/Documents/directory/Sonali_Banerjee_-_Mumbai02012026041155PM.jpg', '3609', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('9984', '9821616296', '91', 'Naveen', NULL, 'Kumar', 'naveen.kumar@irclass.org', NULL, '32182', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('12397', '9840513542', '91', 'Arbind', 'Kumar', 'Choudhary', 'arbind.choudhary34@gov.in', NULL, '35138', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13012', '9869603086', '91', 'Madhura', 'Atesting', 'Testingtestingtestingtestingtestingtesting', 'android@kaizeninfotech.com', 'https://imeiconnect.com/Documents/directory/bird_scenery19012026103731AM.jpg', '23536536363', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13020', '9597056799', '91', 'Mani', NULL, 'Kandan', 'ios@kaizeninfotech.com', 'https://imeiconnect.com/Documents/directory/20022026110531AM.png', '4383', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13021', '9988776655', '91', 'Default', NULL, 'Number', 'ios@kaizeninfotech.com', 'https://imeiconnect.com/Documents/directory/16012026042400PM.png', '28376', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13022', '7358269667', '91', 'Khushboo', 'T', 'Test', 'tester@kaizeninfotech.com', 'https://imeiconnect.com/Documents/directory/18022026050225PM.png', '34356', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13032', '9876543215', '91', 'Android Kai', 'Gd', 'Test', 'sam@gmail.com', 'https://imeiconnect.com/Documents/directory/announcement_bullhorn15012026033841PM.png', '89656656', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13033', '9372778807', '91', 'Maithili', 'Nana', 'Dukhande', 'software@kaizeninfotech.com', NULL, '12345', 1, 1);
INSERT INTO `users` (`Id`, `MobileNo`, `CountryCode`, `FirstName`, `MiddleName`, `LastName`, `Email`, `ProfileImage`, `ImeiMemId`, `IsRegistered`, `IsActive`) VALUES ('13034', '9769063952', '91', 'Jefren', NULL, 'Jef', 'ios@kaizeninfotech.com', 'https://imeiconnect.com/Documents/directory/pexels-jan-tang-598592438-3506354305022026105353AM.jpg', '45434', 1, 1);
-- 34 users inserted

-- ===================== GROUPS =====================
TRUNCATE TABLE `groups`;
INSERT INTO `groups` (`Id`, `GrpName`, `GrpImg`, `GrpType`, `GrpCategory`, `City`, `Country`, `Email`, `Mobile`, `TotalMembers`, `DistrictId`, `IsActive`) VALUES (33359, 'Testing', 'https://imeiconnect.com/Documents/Group/pexels-suzyhazelwood-375530903022026111543AM.jpg', 'Close', '1', 'thane', NULL, NULL, '+91', 7, NULL, 1);
INSERT INTO `groups` (`Id`, `GrpName`, `GrpType`, `TotalMembers`, `DistrictId`, `IsActive`) VALUES (31185, 'National Admin', 'Close', 34, 1, 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33334', 'Chandigarh Chapter', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33335', 'Chennai Branch', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33337', 'Delhi Branch', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33358', 'Goa', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33338', 'Gujarat Chapter', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33344', 'Head Office', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33349', 'Hongkong Residents', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33321', 'Hyderabad Chapter', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33317', 'Karnataka Chapter', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33316', 'Kochi Branch', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33318', 'Kolkata Branch', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33314', 'Mumbai Branch', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33339', 'Navi Mumbai Chapter', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33340', 'Patna Chapter', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33319', 'Pune Branch', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33357', 'Rajasthan', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33350', 'Singapore Residents', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('2765', 'Thane view city', 1);
INSERT INTO `groups` (`Id`, `GrpName`, `IsActive`) VALUES ('33320', 'Visakhapatnam Branch', 1);

-- ===================== GROUP MODULES =====================
TRUNCATE TABLE `group_modules`;
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('529703', '31185', '3', 'Announcements', NULL, 'https://imeiconnect.com/Images/announcemen.png', '13020', 'No', '5', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('529704', '31185', '80', 'iMelange', NULL, 'https://imeiconnect.com/Images/iMelange.png', '13020', 'No', '3', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('529712', '31185', '33', 'Governing council Member', NULL, 'https://imeiconnect.com/Images/committee.png', '13020', 'No', '3', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('656998', '31185', '2', 'Upcoming Events', NULL, 'https://imeiconnect.com/Images/event.png', '13020', 'No', '6', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('857758', '31185', '77', 'Branch & Chapter committees', NULL, 'https://imeiconnect.com/Images/clubs.png', '13020', 'No', '0', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('861445', '31185', '29', 'Past Presidents', NULL, 'https://imeiconnect.com/Images/past_president.png', '13020', 'No', '7', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('861691', '31185', '79', 'Member', NULL, 'https://imeiconnect.com/Images/directory.png', '13020', 'No', '1', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('861929', '31185', '4', 'MER (I)', NULL, 'https://imeiconnect.com/Images/e-bulletin.png', '13020', 'No', '2', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862031', '33359', '1', 'Member', NULL, 'https://imeiconnect.com/Images/directory.png', '13020', 'No', '1', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862032', '33359', '8', 'Past Events', NULL, 'https://imeiconnect.com/Images/event.png', '13020', 'No', '2', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862033', '33359', '3', 'Announcements', NULL, 'https://imeiconnect.com/Images/announcemen.png', '13020', 'No', '3', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862034', '33359', '6', 'Calendar', NULL, 'https://imeiconnect.com/Images/celebration.png', '13020', 'No', '8', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862035', '33359', '78', 'Past Chairman', NULL, 'https://imeiconnect.com/Images/past_president.png', '13020', 'No', '10', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862036', '33359', '26', 'Executive Committee', NULL, 'https://imeiconnect.com/Images/committee.png', '13020', 'No', '11', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862037', '33359', '16', 'Feedback', NULL, 'https://imeiconnect.com/Images/feedback_icon.png', '13020', 'No', '13', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862038', '33359', '9', 'Documents', NULL, 'https://imeiconnect.com/Images/document-safe.png', '13020', 'No', '14', 1);
INSERT INTO `group_modules` (`Id`, `GroupId`, `ModuleId`, `ModuleName`, `ModuleStaticRef`, `Image`, `MasterProfileId`, `IsCustomized`, `ModuleOrderNo`, `IsActive`) VALUES ('862039', '33359', '17', 'Event Attendance', NULL, 'https://imeiconnect.com/Images/attendance.png', '13020', 'No', '107', 1);
-- 17 modules inserted

-- ===================== MEMBER PROFILES =====================
TRUNCATE TABLE `member_profiles`;
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13059', '13020', 'Mani  Kandan', 'ios@kaizeninfotech.com', '9597056799', 'India', 'https://imeiconnect.com/Documents/directory/20022026110531AM.png', 'B -ve', '91', '2026-03-04', '2026-03-04', NULL, 'Fellow', '4383', 'Ashore - Director / CEO/ Owner', '9', '0', '1', '0', 'Kaizen Company');
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13060', '13021', 'Default  Number', 'ios@kaizeninfotech.com', '9988776655', 'India', 'https://imeiconnect.com/Documents/directory/16012026042400PM.png', NULL, '91', '2026-02-25', '2026-02-25', '9876512340', NULL, '28376', 'Ashore - Chief Engineer', '25', '0', '0', '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13066', '1564', 'Iqbal  Singh', 'iqbalsingh363@yahoo.co.in', '9814434912', 'India', 'https://imeiconnect.com/Documents/directory/Iqbal_Singh_-_Chandigarh_Chapter02012026102353AM.jpg', NULL, '91', '2026-01-15', NULL, NULL, 'Fellow', '3866', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13067', '574', 'Sanjeet  Singh', 'sanjeet332@yahoo.co.in', '9815538175', 'India', NULL, NULL, '91', '2026-09-15', NULL, NULL, 'Fellow', '1440', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13068', '1633', 'Ramasamy  Muthusamy', 'rsamy501@gmail.com', '9840134947', 'India', 'https://imeiconnect.com/Documents/directory/R._Muthusamy06112025022457PM.jpg', NULL, '91', '2026-05-06', NULL, NULL, 'Fellow', '3989', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13069', '1927', 'Ramesh  Subramanian', 'chandri.ramesh@gmail.com', '9381013180', 'India', 'https://imeiconnect.com/Documents/directory/._Ramesh_Subramanian-_Chennai02012026104327AM.jpg', NULL, '91', '2026-06-06', NULL, NULL, 'Fellow', '4498', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13070', '3685', 'Dr Sivasami  K', 'ksivasami@imu.ac.in', '9994968456', 'India', NULL, NULL, '91', '2026-06-02', NULL, NULL, 'Fellow', '12482', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13071', '8572', 'Dinesh Karumannan  Ramasamy', 'ram_krdinesh@yahoo.com', '9488035126', 'India', NULL, NULL, '91', '2026-11-21', NULL, NULL, 'Fellow', '25882', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13072', '1101', 'Atul Mani  Sharma', 'atulmani.sharma@gmail.com', '9810363384', 'India', NULL, NULL, '91', '2026-04-21', NULL, NULL, 'Fellow', '2723', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13079', '1328', 'Preetam Kumar Seetharam', 'preetamkumars@gmail.com', '9535248730', 'India', NULL, NULL, '91', '2026-04-27', NULL, NULL, 'Fellow', '3285', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13080', '2265', 'Sajan P.  John', 'sajanp.john@yahoo.co.in', '9895164498', 'India', NULL, NULL, '91', '2026-05-31', NULL, NULL, 'Fellow', '5092', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13081', '1337', 'Sanjeev Kootu Ngal Krishnapillai', 'kksanjeev19@gmail.com', '8879366414', 'India', NULL, NULL, '91', '2026-11-19', NULL, NULL, 'Fellow', '3313', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13094', '2027', 'Dipak  Mohan', 'dipakmohan@gmail.com', '9037477904', 'India', 'https://imeiconnect.com/Documents/directory/Dipak-Mohan_Kochi02012026021922PM.jpg', NULL, '91', '2026-01-27', NULL, NULL, 'Fellow', '4658', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13099', '501', 'Sadhan Kumar Sarkar', 'sksarkarmariner@gmail.com', '9875344077', 'India', 'https://imeiconnect.com/Documents/directory/SK-Sarkar02012026023359PM.jpeg', NULL, '91', '2026-06-01', NULL, NULL, 'Fellow', '1238', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13102', '1961', 'Sanjeev V. Mehra', 'sanjeev.mehra64@gmail.com', '9820432899', 'India', 'https://imeiconnect.com/Documents/directory/Sanjeev_Mehra_-_Mumbai06112025013005PM.jpeg', NULL, '91', '2026-10-17', NULL, NULL, 'Fellow', '4562', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13103', '8835', 'Sonali  Banerjee', 'sonali4843@gmail.com', '9860996837', 'India', 'https://imeiconnect.com/Documents/directory/Sonali_Banerjee_-_Mumbai02012026041155PM.jpg', NULL, '91', '2026-09-26', NULL, NULL, 'Fellow', '3609', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13104', '9984', 'Naveen  Kumar', 'naveen.kumar@irclass.org', '9821616296', 'India', NULL, NULL, '91', '2026-01-01', NULL, NULL, 'Fellow', '32182', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13106', '938', 'Chitta Ranjan Dash', 'chittadash1958@gmail.com', '8879004806', 'India', 'https://imeiconnect.com/Documents/directory/chitta-ranjan-dash02012026042738PM.jpg', NULL, '91', '2026-09-04', NULL, NULL, 'Fellow', '2375', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13107', '2481', 'Mohammed Tarique Mulla', 'tarique_mulla@yahoo.co.in', '9619547597', 'India', 'https://imeiconnect.com/Documents/directory/Tarique_Mulla_-_Navi_Mumbai02012026042829PM.png', NULL, '91', '2026-01-21', NULL, NULL, 'Fellow', '5712', 'Ashore - Chief Engineer', '25', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13108', '7870', 'Jyoti Kumari Nayak', 'seabird_here@yahoo.com', '8169262115', 'India', NULL, NULL, '91', '2026-01-01', NULL, NULL, 'Associate Member', '22979', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13109', '8357', 'Archana Saxena Sangal', 'archana_sangal@yahoo.com', '9920062295', 'India', 'https://imeiconnect.com/Documents/directory/Archana_Saxena_Sangal_-_Navi_Mumbai02012026042953PM.jpg', NULL, '91', '2026-02-15', NULL, NULL, 'Fellow', '24734', 'Ashore - Chief Engineer', '25', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13110', '1359', 'Sanjeev Dinanath Ogale', 'sanjeev@torinomail.com', '9822409657', 'India', NULL, NULL, '91', '2026-06-05', NULL, NULL, 'Fellow', '3346', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13111', '8706', 'Ajit Raghunath Shelar', 'ajitshelar367@gmail.com', '9028470600', 'India', NULL, NULL, '91', '2026-09-30', NULL, NULL, 'Fellow', '27049', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13112', '1430', 'Deepak Vaman Saranjame', 'dsaranjame@gmail.com', '9823895454', 'India', NULL, NULL, '91', '2026-03-25', NULL, NULL, 'Member', '3548', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13113', '2104', 'Mohan Ganesh Joshi', 'mogajoshi@gmail.com', '9423160593', 'India', NULL, NULL, '91', '2026-02-16', NULL, NULL, 'Member', '4775', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13114', '8462', 'Varaha Siva Prasad Vanthala', 'prasadau@gmail.com', '9440413844', 'India', NULL, NULL, '91', '2026-02-01', NULL, NULL, 'Fellow', '24940', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13115', '1204', 'Voona Lakshmipati Rao', 'voonalrao@gmail.com', '9848199565', 'India', NULL, NULL, '91', '2026-01-02', '2026-01-01', NULL, 'Fellow', '2963', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13116', '520', 'Vijayananda Kumar Amara', 'amaravk1954@gmail.com', '9390689530', 'India', NULL, NULL, '91', '2026-08-12', NULL, NULL, 'Fellow', '1283', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13117', '12397', 'Arbind Kumar Choudhary', 'arbind.choudhary34@gov.in', '9840513542', 'India', NULL, NULL, '91', '2026-09-23', NULL, NULL, 'Fellow', '35138', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13141', '13032', 'Android Kai Gd Test', 'sam@gmail.com', '9876543215', 'India', 'https://imeiconnect.com/Documents/directory/announcement_bullhorn15012026033841PM.png', 'AB +ve', '91', '2026-01-27', '2026-01-27', '9876543215', 'Graduate', '89656656', 'Ashore - Naval Architect', '20', '0', '0', '0', 'rerweq');
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13142', '13012', 'Madhura Atesting Testing', 'android@kaizeninfotech.com', '9869603086', 'India', 'https://imeiconnect.com/Documents/directory/bird_scenery19012026103731AM.jpg', 'O +ve', '91', '2026-02-16', '2026-02-16', '9876543210', 'Member', '23536536363', 'Ashore - Retired', '1', '0', '1', '1', 'testing company');
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13143', '13012', 'Madhura Atesting Testingtestingtestingtestingtestingtesting', 'android@kaizeninfotech.com', '9869603086', 'India', 'https://imeiconnect.com/Documents/directory/bird_scenery19012026103731AM.jpg', 'O +ve', '91', '2026-01-01', '2026-01-05', NULL, 'Member', '23536536363', 'Ashore - Retired', '1', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13144', '13033', 'Maithili Nana Dukhande', 'software@kaizeninfotech.com', '9372778807', 'India', NULL, NULL, '91', '2026-02-03', '2026-02-03', NULL, NULL, '12345', NULL, '0', '0', '0', '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13145', '13033', 'Maithili Nana Dukhande', 'software@kaizeninfotech.com', '9372778807', 'India', NULL, NULL, '91', '2026-01-26', '2026-01-26', NULL, NULL, '12345', NULL, '0', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13146', '13032', 'Android Kai Gd Test', 'sam@gmail.com', '9876543215', 'India', 'https://imeiconnect.com/Documents/directory/announcement_bullhorn15012026033841PM.png', 'AB +ve', '91', '2026-01-27', '2026-01-27', '9876543215', 'Graduate', '89656656', 'Ashore - Naval Architect', '20', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13147', '13020', 'Mani  Kandan', 'ios@kaizeninfotech.com', '9597056799', 'India', 'https://imeiconnect.com/Documents/directory/20022026110531AM.png', NULL, '91', NULL, NULL, NULL, NULL, '4383', 'Indian_Navy - Lieutenant Commander', '2', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13148', '13022', 'Khushboo T Testsd', 'tester@kaizeninfotech.com', '7358269667', 'India', 'https://imeiconnect.com/Documents/directory/18022026050225PM.png', 'A +ve', '91', '2026-03-03', '2026-03-03', '9869603086', 'Student', '34356', 'Ashore - Retired', '1', '0', '0', '0', 'Kaizen Infotech');
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13149', '13022', 'Khushboo T Test', 'tester@kaizeninfotech.com', '7358269667', 'India', 'https://imeiconnect.com/Documents/directory/18022026050225PM.png', 'A +ve', '91', '2026-02-03', '2026-02-03', NULL, 'Student', '34356', 'Ashore - Retired', '1', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13150', '13034', 'Jefren  Jef', 'ios@kaizeninfotech.com', '9769063952', 'India', 'https://imeiconnect.com/Documents/directory/pexels-jan-tang-598592438-3506354305022026105353AM.jpg', 'A +ve', '91', '2026-03-04', '2026-03-04', NULL, 'Member', '45434', 'Ashore - Chief Engineer', '25', '0', '0', '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13151', '13021', 'Default  Number', 'ios@kaizeninfotech.com', '9988776655', 'India', 'https://imeiconnect.com/Documents/directory/16012026042400PM.png', NULL, '91', '2026-02-24', '2026-02-25', '9876512340', NULL, '28376', 'Ashore - Chief Engineer', '25', '0', NULL, '0', NULL);
INSERT INTO `member_profiles` (`Id`, `UserId`, `MemberName`, `MemberEmail`, `MemberMobile`, `MemberCountry`, `ProfilePic`, `BloodGroup`, `CountryCode`, `Dob`, `Doa`, `SecondaryMobileNo`, `MembershipGrade`, `ImeiMembershipId`, `Category`, `CategoryId`, `HideNum`, `HideMail`, `HideWhatsnum`, `CompanyName`) VALUES ('13152', '13034', 'Jefren  Jef', 'ios@kaizeninfotech.com', '9769063952', 'India', 'https://imeiconnect.com/Documents/directory/pexels-jan-tang-598592438-3506354305022026105353AM.jpg', 'A +ve', '91', '2026-03-04', '2026-03-04', NULL, 'Member', '45434', 'Ashore - Chief Engineer', '25', '0', NULL, '0', NULL);
-- 41 member profiles inserted

-- ===================== GROUP MEMBERS =====================
TRUNCATE TABLE `group_members`;
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (1, 33359, '13059', '1', 'No', '13020', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (2, 33359, '13060', '1', 'No', '13021', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (3, 33359, '13141', '1', 'No', '13032', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (4, 33359, '13142', '1', 'No', '13012', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (5, 33359, '13144', '1', 'No', '13033', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (6, 33359, '13148', '1', 'Yes', '13022', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (7, 33359, '13150', '1', 'No', '13034', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (8, 31185, '13066', '1', 'No', '1564', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (9, 31185, '13067', '1', 'No', '574', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (10, 31185, '13068', '1', 'No', '1633', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (11, 31185, '13069', '1', 'No', '1927', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (12, 31185, '13070', '1', 'No', '3685', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (13, 31185, '13071', '1', 'No', '8572', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (14, 31185, '13072', '1', 'No', '1101', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (15, 31185, '13079', '1', 'No', '1328', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (16, 31185, '13080', '1', 'No', '2265', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (17, 31185, '13081', '1', 'No', '1337', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (18, 31185, '13094', '1', 'No', '2027', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (19, 31185, '13099', '1', 'No', '501', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (20, 31185, '13102', '1', 'No', '1961', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (21, 31185, '13103', '1', 'No', '8835', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (22, 31185, '13104', '1', 'No', '9984', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (23, 31185, '13106', '1', 'No', '938', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (24, 31185, '13107', '1', 'No', '2481', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (25, 31185, '13108', '1', 'No', '7870', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (26, 31185, '13109', '1', 'No', '8357', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (27, 31185, '13110', '1', 'No', '1359', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (28, 31185, '13111', '1', 'No', '8706', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (29, 31185, '13112', '1', 'No', '1430', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (30, 31185, '13113', '1', 'No', '2104', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (31, 31185, '13114', '1', 'No', '8462', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (32, 31185, '13115', '1', 'No', '1204', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (33, 31185, '13116', '1', 'No', '520', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (34, 31185, '13117', '1', 'No', '12397', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (35, 31185, '13143', '1', 'No', '13012', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (36, 31185, '13145', '1', 'No', '13033', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (37, 31185, '13146', '1', 'No', '13032', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (38, 31185, '13147', '1', 'No', '13020', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (39, 31185, '13149', '1', 'No', '13022', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (40, 31185, '13151', '1', 'No', '13021', 1);
INSERT INTO `group_members` (`Id`, `GroupId`, `MemberProfileId`, `MyCategory`, `IsGrpAdmin`, `MemberMainId`, `IsActive`) VALUES (41, 31185, '13152', '1', 'No', '13034', 1);
-- 41 group memberships inserted

-- ===================== ADDRESS DETAILS =====================
TRUNCATE TABLE `address_details`;
INSERT INTO `address_details` (`Id`, `MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Country`, `Pincode`, `PhoneNo`, `Fax`) VALUES ('13036', '13059', 'Residence', 'Thane', 'Thane', NULL, 'India', '400607', NULL, NULL);
INSERT IGNORE INTO `address_details` (`MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Pincode`) VALUES ('13141', 'Residence', 'mumbai', 'thane', 'Goa', '400602');
INSERT IGNORE INTO `address_details` (`MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Pincode`) VALUES ('13142', 'Residence', 'mumbai', 'thane', 'Goa', '400602');
INSERT IGNORE INTO `address_details` (`MemberProfileId`, `AddressType`, `Address`, `City`, `State`, `Pincode`) VALUES ('13148', 'Residence', 'mumbai', 'thane', 'Goa', '400602');

-- ===================== CLUBS =====================
TRUNCATE TABLE `clubs`;
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (1, '33344', 'Head Office', NULL, NULL, NULL, NULL, '0', '00:00:00', NULL, NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (2, '33349', 'Hongkong Residents', NULL, NULL, NULL, 'India', 'Saturday', '17:20:00', NULL, NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (3, '33350', 'Singapore Residents', NULL, NULL, NULL, 'India', 'Saturday', '17:20:00', NULL, NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (4, '33357', 'Rajasthan', NULL, NULL, NULL, NULL, '0', '00:00:00', NULL, NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (5, '33314', 'Mumbai Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,Sanjeev V. Mehra,Mr Ranjeet Singh  ,Mr. Sunayan Sanatani  ,Mr Lokanath P Tripathy  ,Rajesh  Kasaragod,Pattabhiraman  Lakshman,Sonali  Banerjee,Naveen  Kumar,Bikram Kumar Jena,Cdr Dr. Bhaskar Murlidhar Bhandarkar', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (6, '33316', 'Kochi Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,  ,Sajan P.  John,Mr. Sivaram Narayana Swamy  ,Sanjeev Kootu Ngal Krishnapillai,Mr Isaac Palathinkal Isaac  ,Mr Rajan Neithileth  ,V V  Paul,Dipak  Mohan,Ashraf M Sultan,Jis  George', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (7, '33317', 'Karnataka Chapter', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,Preetam Kumar Seetharam,Ms Rupali Raj Joshi  ,K. T. Prakash  Alva', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (8, '33318', 'Kolkata Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,Sadhan Kumar Sarkar,Ashoke Kumar Barai,Soumitra  Neogi,Arun Kumar Singh,Swapan Kumar Saha,Shanti Ranjan Pal,Gautam  Sen,Abhijit  Banerjee,Saumitra  Ghosh', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (9, '33319', 'Pune Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,Girish Vasant Kotwal,Sanjeev Dinanath Ogale,Ajit Raghunath Shelar,Deepak Vaman Saranjame,Mohan Ganesh Joshi', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (10, '33320', 'Visakhapatnam Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,  ,  ,Dilshah Singh Anand,Varaha Siva Prasad Vanthala,Voona Lakshmipati Rao,Samavedam V. D. Prasad,Vijayananda Kumar Amara', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (11, '33321', 'Hyderabad Chapter', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,Chennuri Siva Ramakrishna Prasad,Gundugurthy Venkataramana Rao,Mr Harsimran Singh', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (12, '33334', 'Chandigarh Chapter', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,Mr. Arun Kumar Agarwal  ,Iqbal  Singh,Sanjeet  Singh,Mr Ramesh Kumar', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (13, '33335', 'Chennai Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,  ,Ramasamy  Muthusamy,Mr. Sadagopan Kannan  ,Ramesh  Subramanian,Dr Sivasami  K,Sanjeev S. Vakil,Suresh Appula Shenoi,Mr Rajesh  Madusudanan,Dinesh Karumannan  Ramasamy', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (14, '33337', 'Delhi Branch', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,  ,  ,  ,Atul Mani  Sharma,Cdr Ashish Mathew  ,Chirag  Bahri,Mr Samir Saran Lal', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (15, '33338', 'Gujarat Chapter', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,Mr Prakash Gajendra Desai  ,Mr Kushal Ashwin Mehta  ,Mr. Puru Dilip Bakshi  ,Mr Saumil Madhusudan Thanki', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (16, '33339', 'Navi Mumbai Chapter', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,  ,  ,  ,Chitta Ranjan Dash,Mohammed Tarique Mulla,Jyoti Kumari Nayak,Archana Saxena Sangal', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (17, '33340', 'Patna Chapter', NULL, NULL, NULL, NULL, '0', '00:00:00', ',  ,', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (18, '33358', 'Goa', 'IMEI HOUSE, D 27, RANGAVI ESTATE, DABOLIM, GOA', 'GOA', 'GOA', NULL, '0', '00:00:00', 'Larson  Dsa,Shivram Ravindra  Kamat,Alwin Anthony Dias,Hrushikesh  Sahu,Bhaskar Prabhaker Rivanker', NULL);
INSERT INTO `clubs` (`Id`, `GroupId`, `ClubName`, `Address`, `City`, `State`, `Country`, `MeetingDay`, `MeetingTime`, `PresidentName`, `SecretaryName`) VALUES (19, '33359', 'Testing', 'Thane', 'thane', 'Maharashtra', NULL, 'Monday', '16:30:00', 'Madhura  Android,Madhura  Android,Maithili Nana Dukhande,Android Kai Gd Test,Khushboo Sunil Yadav,Madhura Atesting Testing,Abc Abc Abc,Abc Abc Abc,Mani  Kandan', NULL);
-- 19 clubs inserted

-- ===================== ATTENDANCE RECORDS =====================
TRUNCATE TABLE `attendance_records`;
TRUNCATE TABLE `attendance_members`;
TRUNCATE TABLE `attendance_visitors`;
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (45, 33359, '24 Events Attend', '02 March 2026', '11:45:00 AM', 'Representation: An ideal sample reflects the characteristics of the larger population to draw accurate conclusions.\nSampling Methods: Techniques include probability (random) sampling, ensuring an unbiased selection, and non-probability sampling.\nPurpose: Used in studies, scientific research, data collection, and market research to make informed decisions.\nExamples: A small, free quantity of a product for trial, a group of consumers for a survey, or a selection of data points for analysis.', 13059);
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (45, '13141', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (45, '13060', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (45, '13150', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (45, '13148', '1');
INSERT INTO `attendance_visitors` (`Id`, `AttendanceRecordId`, `VisitorName`, `Type`) VALUES ('35317', '45', 'Poonam', '2');
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (44, 33359, 'demo', '04 February 2026', '09:52:00 AM', 'demo desc', 13059);
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (44, '13141', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (44, '13150', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (44, '13148', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (44, '13142', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (44, '13144', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (44, '13059', '1');
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (36, 33359, 'abc test', '03 February 2026', '10:45:00 AM', 'sjgiludcgipdu', 13059);
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (36, '13139', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (36, '13141', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (36, '13148', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (36, '13142', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (36, '13144', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (36, '13059', '1');
INSERT INTO `attendance_visitors` (`Id`, `AttendanceRecordId`, `VisitorName`, `Type`) VALUES ('35315', '36', 'test1', '2');
INSERT INTO `attendance_visitors` (`Id`, `AttendanceRecordId`, `VisitorName`, `Type`) VALUES ('35316', '36', 'test2', '2');
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (35, 33359, 'khush', '02 February 2026', '07:15:00 PM', 'test', 13059);
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (35, '13141', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (35, '13150', '1');
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (34, 33359, 'Test Attend', '08 January 2026', '05:20:00 PM', 'android', 13059);
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (34, '13141', '1');
INSERT INTO `attendance_members` (`AttendanceRecordId`, `MemberProfileId`, `Type`) VALUES (34, '13150', '1');
INSERT INTO `attendance_visitors` (`Id`, `AttendanceRecordId`, `VisitorName`, `Type`) VALUES ('35313', '34', 'Sam', '2');
INSERT INTO `attendance_visitors` (`Id`, `AttendanceRecordId`, `VisitorName`, `Type`) VALUES ('35314', '34', 'Tina', '2');
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (41, 33359, 'demo1', '04 February 2025', '11:34:00 AM', 'demo1', 13059);
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (38, 33359, 'fsdge', '04 February 2025', '10:44:00 AM', 'ewfbewb', 13059);
INSERT INTO `attendance_records` (`Id`, `GroupId`, `AttendanceName`, `AttendanceDate`, `AttendanceTime`, `AttendanceDesc`, `CreatedBy`) VALUES (40, 33359, 'demo', '03 February 2025', '11:24:00 AM', 'demo', 13059);
-- 8 attendance records inserted

-- ===================== PAST PRESIDENTS =====================
TRUNCATE TABLE `past_presidents`;
INSERT INTO `past_presidents` (`Id`, `GroupId`, `MemberName`, `PhotoPath`, `TenureYear`) VALUES ('227', 33359, 'Tesand', 'https://imeiconnect.com/Documents/pastpresidents/Group33359/bird_scenery14012026105424AM.jpg', '2026');
INSERT INTO `past_presidents` (`Id`, `GroupId`, `MemberName`, `PhotoPath`, `TenureYear`) VALUES ('228', 33359, 'Khushboo Yadav', 'https://imeiconnect.com/Documents/pastpresidents/Group33359/pexels-2158114840-3516932002022026070730PM.jpg', '2026');
INSERT INTO `past_presidents` (`Id`, `GroupId`, `MemberName`, `PhotoPath`, `TenureYear`) VALUES ('230', 33359, 'test khushdfdg', 'https://imeiconnect.com/Documents/pastpresidents/Group33359/29122025111705AM03022026103222AM.jpg', '2026');
INSERT INTO `past_presidents` (`Id`, `GroupId`, `MemberName`, `PhotoPath`, `TenureYear`) VALUES ('234', 33359, 'abcd12', 'https://imeiconnect.com/Documents/pastpresidents/Group33359/pexels-kpaukshtite-142785524022026121429PM.jpg', '2026');
-- 4 past presidents inserted

-- ===================== TOUCHBASE SETTINGS =====================
TRUNCATE TABLE `touchbase_settings`;
INSERT INTO `touchbase_settings` (`Id`, `UserId`, `GrpId`, `GrpVal`, `GrpName`) VALUES (1, 13020, '33359', '1', 'Testing');
INSERT INTO `touchbase_settings` (`Id`, `UserId`, `GrpId`, `GrpVal`, `GrpName`) VALUES (2, 13020, '33359', '1', 'Testing');

-- ===================== BOD DESIGNATIONS (update group_members) =====================
UPDATE `member_profiles` SET `Designation` = 'President' WHERE `Id` = '13142';
UPDATE `member_profiles` SET `Designation` = 'Hon. Secretary' WHERE `Id` = '13144';
UPDATE `member_profiles` SET `Designation` = 'President' WHERE `Id` = '13059';
UPDATE `member_profiles` SET `Designation` = 'Hon. Secretary' WHERE `Id` = '13141';
-- 4 BOD designations updated

-- ===================== BANNERS =====================
TRUNCATE TABLE `banners`;
-- No banner data returned from API (dashboard empty)

-- ===================== NOTIFICATIONS =====================
TRUNCATE TABLE `notifications`;
-- Notifications are user-specific and transient


SET FOREIGN_KEY_CHECKS = 1;

-- ============================================================
-- Migration complete!
-- ============================================================
