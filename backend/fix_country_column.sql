-- =====================================================================
-- Fix address_details.Country column
-- Generated: 2026-04-10 15:02:01
-- Strategy: for each misclassified Country value, move it into State
--           (only if State is currently empty), then NULL out Country.
-- =====================================================================
USE `imei_new`;
SET sql_mode = '';
SET autocommit = 0;
START TRANSACTION;
SET @now = NOW();

-- 1431 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MUMBAI';
-- 1042 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERALA';
-- 824 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'GOA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'GOA';
-- 690 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tamil Nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tamil Nadu';
-- 626 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTAR PRADESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTAR PRADESH';
-- 551 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PUNE';
-- 547 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHARASHTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHARASHTRA';
-- 547 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAVI MUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAVI MUMBAI';
-- 446 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chennai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chennai';
-- 424 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'WEST BENGAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'WEST BENGAL';
-- 340 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Karnataka';
-- 331 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bihar';
-- 263 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NEW DELHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NEW DELHI';
-- 229 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'HARYANA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'HARYANA';
-- 229 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOLKATA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOLKATA';
-- 223 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RAJASTHAN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RAJASTHAN';
-- 204 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gujarat';
-- 191 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Andhra Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Andhra Pradesh';
-- 186 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PUNJAB'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PUNJAB';
-- 178 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tamilnadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tamilnadu';
-- 164 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DELHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DELHI';
-- 126 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jharkhand';
-- 122 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MADHYA PRADESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MADHYA PRADESH';
-- 122 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOCHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOCHI';
-- 117 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Himachal Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Himachal Pradesh';
-- 77 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTARAKHAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTARAKHAND';
-- 71 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BANGALORE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BANGALORE';
-- 58 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Visakhapatnam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Visakhapatnam';
-- 42 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jammu & Kashmir'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jammu & Kashmir';
-- 41 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TELANGANA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TELANGANA';
-- 41 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Patna'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Patna';
-- 36 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THANE';
-- 33 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UNION TERRITORY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UNION TERRITORY';
-- 30 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'LUCKNOW'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'LUCKNOW';
-- 30 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pune, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pune, Maharashtra';
-- 28 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mangalore'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mangalore';
-- 27 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bengaluru'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bengaluru';
-- 27 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Odisha';
-- 22 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mumbai, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mumbai, Maharashtra';
-- 22 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chandigarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chandigarh';
-- 20 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ORISSA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ORISSA';
-- 18 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Assam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Assam';
-- 18 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERALA.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERALA.';
-- 16 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Patna, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Patna, Bihar';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAGPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAGPUR';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Lakshadweep'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Lakshadweep';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THRISSUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THRISSUR';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ernakulam, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ernakulam, Kerala';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTARKHAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTARKHAND';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Lucknow, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Lucknow, Uttar Pradesh';
-- 15 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTARANCHAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTARANCHAL';
-- 14 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'HYDERABAD'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'HYDERABAD';
-- 14 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ernakulam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ernakulam';
-- 12 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Uttarpradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Uttarpradesh';
-- 11 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST THANE';
-- 11 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SATARA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SATARA';
-- 11 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOZHIKODE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOZHIKODE';
-- 10 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NASHIK'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NASHIK';
-- 10 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kanpur, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kanpur, Uttar Pradesh';
-- 10 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kolkata, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kolkata, West Bengal';
-- 10 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kanpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kanpur';
-- 10 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chattisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chattisgarh';
-- 9 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thrissur, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thrissur, Kerala';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Palakkad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Palakkad';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Navi Mumbai, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Navi Mumbai, Maharashtra';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jaipur, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jaipur, Rajasthan';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ranchi, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ranchi, Jharkhand';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chhattisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chhattisgarh';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ghaziabad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ghaziabad';
-- 8 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDAMAN & NICOBAR ISLAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDAMAN & NICOBAR ISLAND';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SOLAPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SOLAPUR';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHARASHTARA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHARASHTARA';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kozhikode, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kozhikode, Kerala';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Alappuzha, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Alappuzha, Kerala';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kottayam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kottayam';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Uttrakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Uttrakhand';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Varanasi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Varanasi';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'CHHATISGARH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'CHHATISGARH';
-- 7 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'GURGAON'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'GURGAON';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'AHMEDNAGAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'AHMEDNAGAR';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'COCHIN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'COCHIN';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kollam, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kollam, Kerala';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Alappuzha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Alappuzha';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jammu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jammu';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'South Goa'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'South Goa';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PUDUCHERRY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PUDUCHERRY';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ramanathapuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ramanathapuram';
-- 6 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DEHRA DUN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DEHRA DUN';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PALGHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PALGHAR';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhopal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhopal';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kerela'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kerela';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERALA STATE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERALA STATE';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mangaluru'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mangaluru';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MYSORE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MYSORE';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'WEST BANGAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'WEST BANGAL';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dehradun, Uttarakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dehradun, Uttarakhand';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'New Delhi, Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'New Delhi, Delhi';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jammu and Kashmir'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jammu and Kashmir';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'AHMEDABAD'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'AHMEDABAD';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Coimbatore'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Coimbatore';
-- 5 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'North Goa'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'North Goa';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOLHAPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOLHAPUR';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NASHIK, MAHARASHTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NASHIK, MAHARASHTRA';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MADHYAPRADESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MADHYAPRADESH';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHARASTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHARASTRA';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TRIVANDRUM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TRIVANDRUM';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KANNUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KANNUR';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kochi, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kochi, Kerala';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Malappuram, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Malappuram, Kerala';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thane, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thane, Maharashtra';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thiruvananthapuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thiruvananthapuram';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOLLAM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOLLAM';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MANIPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MANIPUR';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jamshedpur, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jamshedpur, Jharkhand';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mangalore, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mangalore, Karnataka';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhopal, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhopal, Madhya Pradesh';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Shimla, Himachal Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Shimla, Himachal Pradesh';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nagpur, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nagpur, Maharashtra';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Udupi, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Udupi, Karnataka';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kolhapur, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kolhapur, Maharashtra';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mumbai , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mumbai , Maharashtra';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Raigad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Raigad';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dehradun'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dehradun';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sindhudurg'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sindhudurg';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vasai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vasai';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'madurai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'madurai';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tirunelveli'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tirunelveli';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'JAMMU KASHMIR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'JAMMU KASHMIR';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'JAMSHEDPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'JAMSHEDPUR';
-- 4 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Virudhunagar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Virudhunagar';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST: THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST: THANE';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THANE DISTT'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THANE DISTT';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN WEST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN WEST';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVLI (E)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVLI (E)';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MHARASHTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MHARASHTRA';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RATNAGIRI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RATNAGIRI';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'U.P'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'U.P';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARNATAKA STATE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARNATAKA STATE';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kottayam, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kottayam, Kerala';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kannur, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kannur, Kerala';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Satara, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Satara, Maharashtra';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'kasaragod'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'kasaragod';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jaipur , Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jaipur , Rajasthan';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Lucknow , Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Lucknow , Uttar Pradesh';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ghaziabad, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ghaziabad, Uttar Pradesh';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Delhi, Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Delhi, Delhi';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ranchi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ranchi';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Udupi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Udupi';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tiruvarur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tiruvarur';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vadodara'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vadodara';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NEW PANVEL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NEW PANVEL';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kangra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kangra';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'CHENNAI,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'CHENNAI,';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SOUTH ANDAMAN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SOUTH ANDAMAN';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDAMAN AND NICOBAR ISLANDS'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDAMAN AND NICOBAR ISLANDS';
-- 3 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTAR PRADESHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTAR PRADESHI';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'AMBERNATH (EAST)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'AMBERNATH (EAST)';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'JALGAON'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'JALGAON';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MIRA ROAD (EAST)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MIRA ROAD (EAST)';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'VASAI ( WEST ) THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'VASAI ( WEST ) THANE';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHRASHTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHRASHTRA';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'AURANGABAD'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'AURANGABAD';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN ( WEST )'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN ( WEST )';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHARSHTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHARSHTRA';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pathanamthitta, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pathanamthitta, Kerala';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mahendergarh, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mahendergarh, Haryana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Malappuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Malappuram';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Maradu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Maradu';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pathanamthitta'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pathanamthitta';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vadakara'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vadakara';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kodagu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kodagu';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARWAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARWAR';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARATAKA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARATAKA';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARANATAKA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARANATAKA';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Howrah'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Howrah';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ODISSA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ODISSA';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT - PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT - PUNE';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Hamirpur , Himachal Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Hamirpur , Himachal Pradesh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bokaro Steel City, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bokaro Steel City, Jharkhand';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dewas, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dewas, Madhya Pradesh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bangalore, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bangalore, Karnataka';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pune , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pune , Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dalli Rajhara, Chhattisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dalli Rajhara, Chhattisgarh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gurgaon, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gurgaon, Haryana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bangalore , Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bangalore , Karnataka';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Noida, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Noida, Uttar Pradesh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chennai, Tamil Nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chennai, Tamil Nadu';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Badlapur, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Badlapur, Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhilai, Chhattisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhilai, Chhattisgarh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kolkata , West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kolkata , West Bengal';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Panchkula, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Panchkula, Haryana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Latur, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Latur, Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kharghar, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kharghar, Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kota, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kota, Rajasthan';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Trivandrum, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Trivandrum, Kerala';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Amravati, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Amravati, Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vasai, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vasai, Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dehradun , Uttarakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dehradun , Uttarakhand';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jhunjhunu, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jhunjhunu, Rajasthan';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jodhpur, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jodhpur, Rajasthan';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tuticorin, Tamil Nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tuticorin, Tamil Nadu';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Visakhapatnam, Andhra Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Visakhapatnam, Andhra Pradesh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ahmedabad, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ahmedabad, Gujarat';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Aurangabad, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Aurangabad, Maharashtra';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Faridabad, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Faridabad, Haryana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Aligarh, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Aligarh, Uttar Pradesh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mysore, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mysore, Karnataka';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Varanasi, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Varanasi, Uttar Pradesh';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gaya, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gaya, Bihar';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Alwar, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Alwar, Rajasthan';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sonipat, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sonipat, Haryana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gurugram, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gurugram, Haryana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nowgong'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nowgong';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Barabanki'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Barabanki';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Wai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Wai';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhagalpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhagalpur';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kalyan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kalyan';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Phaltan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Phaltan';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vapi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vapi';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Uttara Kannada, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Uttara Kannada, Karnataka';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dadra and Nagar Haveli'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dadra and Nagar Haveli';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pimpri Chinchwad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pimpri Chinchwad';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dhule'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dhule';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Shimla'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Shimla';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chiplun'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chiplun';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Badlapur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Badlapur';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Panvel'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Panvel';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ghazipur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ghazipur';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chhatrapati Sambhaji Nagar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chhatrapati Sambhaji Nagar';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Meerut'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Meerut';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sawantwadi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sawantwadi';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDHRAPRADESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDHRAPRADESH';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'J&K'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'J&K';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ludhiana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ludhiana';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Patiala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Patiala';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TRICHY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TRICHY';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDMAN & NICOBAR ISLAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDMAN & NICOBAR ISLAND';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PORT BLAIR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PORT BLAIR';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PONDICHERRY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PONDICHERRY';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDAMAN & NICOBAR ISLANDS'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDAMAN & NICOBAR ISLANDS';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'A & N ISLAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'A & N ISLAND';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Villupuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Villupuram';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'A.P.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'A.P.';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DINDIGUL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DINDIGUL';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tuticorin'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tuticorin';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nagapattinam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nagapattinam';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMIL,NADU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMIL,NADU';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THOOTHUKUDI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THOOTHUKUDI';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sivagangai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sivagangai';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thanjavur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thanjavur';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'GREATER NOIDA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'GREATER NOIDA';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAVIMUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAVIMUMBAI';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAVI MUMABI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAVI MUMABI';
-- 2 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'JAIPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'JAIPUR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'GANGAPUR ROAD,NASHIK'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'GANGAPUR ROAD,NASHIK';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THANE DIST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THANE DIST';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MUM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MUM';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THANE (W)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THANE (W)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAGPUR (MAHARASHTRA)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAGPUR (MAHARASHTRA)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN (WEST), DIST : THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN (WEST), DIST : THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'M.S'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'M.S';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST PALGHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST PALGHAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SANGAMNAGAR, SARTATA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SANGAMNAGAR, SARTATA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MIRA ROAD (E) THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MIRA ROAD (E) THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'I.T.I ROAD. SATARA (MAHARASHTRA)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'I.T.I ROAD. SATARA (MAHARASHTRA)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN (E), THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN (E), THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ULHASNAGAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ULHASNAGAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVILI EAST, THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVILI EAST, THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST PALGHAR. THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST PALGHAR. THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST-THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST-THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RAIGAD DISTRICT'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RAIGAD DISTRICT';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'City : Ahmednagar,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'City : Ahmednagar,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT: THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT: THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'AHMEDNAGAR (M.S.)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'AHMEDNAGAR (M.S.)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THANE DIST (M.S.)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THANE DIST (M.S.)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PEN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PEN';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'VIRAR (WEST)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'VIRAR (WEST)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'VASAI (W)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'VASAI (W)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVLE ( EAST )'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVLE ( EAST )';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN (WEST)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN (WEST)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NASIK, (MAHARASHTRA)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NASIK, (MAHARASHTRA)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVLI (WEST)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVLI (WEST)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NALASOPARA ( EAST )'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NALASOPARA ( EAST )';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Andheri West'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Andheri West';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST WARDHA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST WARDHA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN - WEST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN - WEST';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN - EAST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN - EAST';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVLI ( WEST )'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVLI ( WEST )';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVLI ( EAST )'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVLI ( EAST )';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT - THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT - THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MUMBAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MUMBAL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT: PALGHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT: PALGHAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'STATE: DAMOH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'STATE: DAMOH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SANGLI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SANGLI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'WARDHA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'WARDHA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT: RATNAGIRI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT: RATNAGIRI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MADHYA PREDESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MADHYA PREDESH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NASIK (MAHARASHTRA)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NASIK (MAHARASHTRA)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT : THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT : THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DAMAN & DIU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DAMAN & DIU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dist-Satara'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dist-Satara';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHARASNTRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHARASNTRA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DOMBIVILI EAST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DOMBIVILI EAST';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'A.P'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'A.P';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Maharasthra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Maharasthra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ERNAKULAM DISTT., KOCHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ERNAKULAM DISTT., KOCHI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ERNAKULAM (DIST), KERALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ERNAKULAM (DIST), KERALA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERALA,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERALA,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TRIPUNITHURA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TRIPUNITHURA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), '.KOCHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = '.KOCHI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ernakulam North'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ernakulam North';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'U.T Of.Lakshadweep'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'U.T Of.Lakshadweep';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dist.Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dist.Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Palakkad, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Palakkad, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mayiladuthurai, Tamilnadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mayiladuthurai, Tamilnadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Idukki, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Idukki, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mumbai Suburban, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mumbai Suburban, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kanyakumari, Tamilnadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kanyakumari, Tamilnadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'North 24 Parganas, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'North 24 Parganas, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'South 24 Parganas, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'South 24 Parganas, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kasaragod, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kasaragod, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chennai, Tamilnadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chennai, Tamilnadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kutch, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kutch, Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Idukki'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Idukki';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mancode'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mancode';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pathanapuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pathanapuram';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kottuvalikkad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kottuvalikkad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOLAPPURAM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOLAPPURAM';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mannarkkad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mannarkkad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Eloor'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Eloor';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Paniely'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Paniely';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kattanam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kattanam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Changanaserry'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Changanaserry';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Muvattupuzha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Muvattupuzha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'GARAGE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'GARAGE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ERODE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ERODE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Trichur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Trichur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Trissur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Trissur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tinsukia'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tinsukia';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Uttara kannada'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Uttara kannada';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Payyannur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Payyannur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Perunad, Pathanamthitta'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Perunad, Pathanamthitta';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mysuru'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mysuru';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'HUBLI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'HUBLI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BELAGAVI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BELAGAVI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MANGALURU CITY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MANGALURU CITY';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MNGALORE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MNGALORE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PURAM MYSORE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PURAM MYSORE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UDUPI DIST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UDUPI DIST';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BIJAPUR ( DIST )'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BIJAPUR ( DIST )';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Panambur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Panambur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOLKATA (W.B.)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOLKATA (W.B.)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST- HOWRAH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST- HOWRAH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'WEST BRNGAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'WEST BRNGAL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BIRBHUM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BIRBHUM';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SOUTH 24 PARGANAS'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SOUTH 24 PARGANAS';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'WESTBENGAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'WESTBENGAL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'P.S.KUDHANI, BIHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'P.S.KUDHANI, BIHAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST : PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST : PUNE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST ; PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST ; PUNE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOTHRUD, PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOTHRUD, PUNE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BOKARD'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BOKARD';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'LONAVALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'LONAVALA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PUNR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PUNR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DISTRICT: PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DISTRICT: PUNE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST PUNE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST PUNE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Alapuzha, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Alapuzha, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ghaziabad, Uttarpradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ghaziabad, Uttarpradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tundla, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tundla, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bodh Gaya, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bodh Gaya, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mukerian, Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mukerian, Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Delhi , Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Delhi , Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Raipur, Chattisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Raipur, Chattisgarh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kottarakara, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kottarakara, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mananthavady, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mananthavady, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Koderma, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Koderma, Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gurgaon , Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gurgaon , Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Satna, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Satna, Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Konnagar , West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Konnagar , West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gorakhpur , Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gorakhpur , Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Siliguri, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Siliguri, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nabha , Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nabha , Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Guwahati, Assam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Guwahati, Assam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Daltonganj, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Daltonganj, Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Asansol, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Asansol, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Baripada, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Baripada, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Udaipur, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Udaipur, Rajasthan';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dombivli , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dombivli , Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dabhol, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dabhol, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Koparkhairane, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Koparkhairane, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Moolamattom, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Moolamattom, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dhar, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dhar, Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jagatsinghpur, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jagatsinghpur, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chakradharpur, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chakradharpur, Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mundkur, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mundkur, Karnataka';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kanpur, Uttarpradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kanpur, Uttarpradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kudal, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kudal, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Rohtak, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Rohtak, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Lohardaga, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Lohardaga, Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'New Delhi, New Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'New Delhi, New Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Narayangaon, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Narayangaon, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Balaghat , Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Balaghat , Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Durgapur, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Durgapur, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sambalpur, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sambalpur, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pachora, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pachora, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Haldwani, Uttarakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Haldwani, Uttarakhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chapra, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chapra, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sheikhpura, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sheikhpura, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Navi Mumbai , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Navi Mumbai , Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Trivandrum , Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Trivandrum , Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Near Ramlala Mandir, State'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Near Ramlala Mandir, State';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mumbra, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mumbra, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dombivli, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dombivli, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Meerut, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Meerut, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jath, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jath, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Greater Noida, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Greater Noida, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ajmer , Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ajmer , Rajasthan';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Hubli , Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Hubli , Karnataka';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Alappuzha , Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Alappuzha , Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Daspalla, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Daspalla, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chandigarh, Chandigarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chandigarh, Chandigarh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Otur , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Otur , Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Samastipur, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Samastipur, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bijnor , Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bijnor , Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Navi-Mumbai, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Navi-Mumbai, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jammu, Jammu And Kashmir'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jammu, Jammu And Kashmir';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Areraj , Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Areraj , Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhubaneswar, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhubaneswar, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Talwara, Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Talwara, Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chittorgarh , Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chittorgarh , Rajasthan';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kancharapara, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kancharapara, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chhatarpur, Plamau, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chhatarpur, Plamau, Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ratnagiri, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ratnagiri, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vadodara, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vadodara, Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ambala Cantt, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ambala Cantt, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jhansi, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jhansi, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Deoghar, Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Deoghar, Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bareilly, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bareilly, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Rourkela, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Rourkela, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Panipat, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Panipat, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Rourkela , Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Rourkela , Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jalandhar, Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jalandhar, Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chandannagar , West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chandannagar , West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chakur, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chakur, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nagda, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nagda, Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gondia, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gondia, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Fatehabad, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Fatehabad, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Motihari, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Motihari, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dehri On Sone, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dehri On Sone, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vishakapatnam, Andhra Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vishakapatnam, Andhra Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ajmer, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ajmer, Rajasthan';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'East Delhi, New Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'East Delhi, New Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tenali, Andhra Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tenali, Andhra Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Beawar, Rajsthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Beawar, Rajsthan';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Makhdumpur, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Makhdumpur, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Iritty, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Iritty, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Janakpuri, Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Janakpuri, Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Begusarai, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Begusarai, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pimpri Chinchwad, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pimpri Chinchwad, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ahmednagar, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ahmednagar, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sasaram, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sasaram, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhagalpur , Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhagalpur , Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Madhubani, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Madhubani, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vaijapur, Dist Aurangabad, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vaijapur, Dist Aurangabad, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Badnawar, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Badnawar, Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jammu, Jammu & Kashmir'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jammu, Jammu & Kashmir';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ambernath, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ambernath, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mandya, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mandya, Karnataka';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sangli, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sangli, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Amritsar, Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Amritsar, Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gandhidham , Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gandhidham , Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sikandrabad , Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sikandrabad , Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pratapgarh, Rajasthan'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pratapgarh, Rajasthan';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nagercoil, Tamilnadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nagercoil, Tamilnadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Wai, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Wai, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Delhi, Nct Of Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Delhi, Nct Of Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pratapgarh, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pratapgarh, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ahmedabad , Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ahmedabad , Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jammu , Jammu And Kashmir'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jammu , Jammu And Kashmir';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bharuch, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bharuch, Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Raebareli, Uttarpradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Raebareli, Uttarpradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Patiala, Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Patiala, Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pune, Maharastra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pune, Maharastra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Udupi , Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Udupi , Karnataka';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhavnager, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhavnager, Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thana Bihpur, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thana Bihpur, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kalyan West, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kalyan West, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ranchi , Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ranchi , Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Roorkee, Uttarakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Roorkee, Uttarakhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Burdwan, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Burdwan, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Panaji, Goa'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Panaji, Goa';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhagalpur, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhagalpur, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vadodara , Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vadodara , Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Karur, Tamil Nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Karur, Tamil Nadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jind, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jind, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Saoner,Maharashtra, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Saoner,Maharashtra, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Koraput, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Koraput, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Yamuna Nagar, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Yamuna Nagar, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gorakhpur, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gorakhpur, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bokaro Steel City , Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bokaro Steel City , Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kharghar Navi Mumbai, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kharghar Navi Mumbai, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Siwan, Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Siwan, Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mattannur,Kerala, Kerala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mattannur,Kerala, Kerala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Monteswar, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Monteswar, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhowali, Uttarakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhowali, Uttarakhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Virar, Maharastra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Virar, Maharastra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ganj Basoda, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ganj Basoda, Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Khanna, Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Khanna, Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Srungavarapukota, Andhra Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Srungavarapukota, Andhra Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kangra, Himachal Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kangra, Himachal Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Port Blair, Andaman And Nicobar Islands'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Port Blair, Andaman And Nicobar Islands';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gohana, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gohana, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Singrauli, Madhya Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Singrauli, Madhya Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Angul, Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Angul, Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhiwani, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhiwani, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thane , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thane , Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dehra Dun , Uttarakhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dehra Dun , Uttarakhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Patna , Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Patna , Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Akola, Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Akola, Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ghoghla, Daman And Diu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ghoghla, Daman And Diu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jam Nagar, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jam Nagar, Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhubaneswar , Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhubaneswar , Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kalyan , Maharashtra'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kalyan , Maharashtra';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Prayagraj, Uttar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Prayagraj, Uttar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Porbandar, Gujarat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Porbandar, Gujarat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jhajjar, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jhajjar, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Shahdara, Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Shahdara, Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Balangir , Odisha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Balangir , Odisha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Aurangabad(Bh), Bihar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Aurangabad(Bh), Bihar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Belgaum, Karnataka'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Belgaum, Karnataka';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Amritsar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Amritsar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Khadkoli'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Khadkoli';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Banglore'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Banglore';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mira Road , Thane'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mira Road , Thane';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Satana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Satana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dapoli'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dapoli';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sultanpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sultanpur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Shrirampur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Shrirampur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Islampur - Mh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Islampur - Mh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Navsari (Gujarat)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Navsari (Gujarat)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jhajjar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jhajjar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ichagarh/Chandil/Jharkhand'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ichagarh/Chandil/Jharkhand';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bettiah'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bettiah';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Garhdiwala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Garhdiwala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gopalganj'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gopalganj';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thane , Mumbai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thane , Mumbai';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Babhulgaon'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Babhulgaon';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jadugoda'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jadugoda';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Akluj'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Akluj';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Miraj'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Miraj';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pandharpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pandharpur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Purulia, West Bengal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Purulia, West Bengal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Palwal, Haryana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Palwal, Haryana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Uattar Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Uattar Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'East Delhi, Delhi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'East Delhi, Delhi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Uttarâ Pradesh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Uttarâ Pradesh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Meghalaya'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Meghalaya';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mandya'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mandya';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jalandhar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jalandhar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'East Champaran'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'East Champaran';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mizoram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mizoram';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pathankot'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pathankot';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mohali'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mohali';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mohali(Sas Nagar)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mohali(Sas Nagar)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Chttisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Chttisgarh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dombivili West'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dombivili West';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dist Raigad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dist Raigad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mangaluru(Mangalore)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mangaluru(Mangalore)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Palanpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Palanpur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Karnal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Karnal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Navsari'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Navsari';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Deoria'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Deoria';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mahabaleshwar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mahabaleshwar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Harda'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Harda';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ahmadnagar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ahmadnagar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Faridabad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Faridabad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ambernath'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ambernath';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Latehar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Latehar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KHAMGAON'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KHAMGAON';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Barsar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Barsar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Khandala'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Khandala';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ALIBAG'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ALIBAG';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Borivali'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Borivali';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Palampur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Palampur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mira-Bhayandar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mira-Bhayandar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Navi Mumabai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Navi Mumabai';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Lunawada'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Lunawada';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Guntakal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Guntakal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mumai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mumai';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Osmanabad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Osmanabad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SIVAKASI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SIVAKASI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Malshiras'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Malshiras';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kurkumb'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kurkumb';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sankarankoil'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sankarankoil';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jamner'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jamner';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Boisar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Boisar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dist- Muzaffarpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dist- Muzaffarpur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Oddanchatram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Oddanchatram';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kasargod'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kasargod';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'AGRA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'AGRA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Rajapalayam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Rajapalayam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALYAN EAST'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALYAN EAST';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST : THANE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST : THANE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BIHHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BIHHAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kondagaon , Chhattisgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kondagaon , Chhattisgarh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nainital'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nainital';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sonitpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sonitpur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Udupi District'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Udupi District';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bardhaman'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bardhaman';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Dewas'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Dewas';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Amravati'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Amravati';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'West Champaran'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'West Champaran';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ayanavaram,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ayanavaram,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bengaluru Urban'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bengaluru Urban';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Faizabad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Faizabad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Surat'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Surat';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'North 24 Parganas'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'North 24 Parganas';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Alwar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Alwar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RAIPUR (C.G.)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RAIPUR (C.G.)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'GUNTUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'GUNTUR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'VISHAKHAPATNAM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'VISHAKHAPATNAM';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDHRA PRDESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDHRA PRDESH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDHRA PRADES'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDHRA PRADES';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Visakhaptnam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Visakhaptnam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SAS NAGAR, PUNJAB'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SAS NAGAR, PUNJAB';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'HIMACHAL RADESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'HIMACHAL RADESH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'CHENNNAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'CHENNNAI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PONDICHERY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PONDICHERY';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'U.T Of Puducherry.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'U.T Of Puducherry.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARAIKAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARAIKAL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THIRUVARUR (DT)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THIRUVARUR (DT)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PORT BLAIR, S. ANDAMAN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PORT BLAIR, S. ANDAMAN';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAGAPATTINAM,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAGAPATTINAM,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PUDUCHERY'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PUDUCHERY';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DINDIGUL (DT)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DINDIGUL (DT)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDAMAN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDAMAN';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tanil nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tanil nadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMIL NADU,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMIL NADU,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMILNBADU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMILNBADU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Daman and Diu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Daman and Diu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMIL NAD'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMIL NAD';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOLLAM,KERALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOLLAM,KERALA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'CUDDALORE'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'CUDDALORE';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERALA,678601'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERALA,678601';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'STATE-JHARKHAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'STATE-JHARKHAND';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THIRUVANANTHAPURAM, KERALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THIRUVANANTHAPURAM, KERALA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THIRUVANANTHAPURAM,KERALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THIRUVANANTHAPURAM,KERALA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THRISSUR (D)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THRISSUR (D)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'STATE- KARNATAKA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'STATE- KARNATAKA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERALA - ST, 679333'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERALA - ST, 679333';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KHAMMAM,TELANGANA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KHAMMAM,TELANGANA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDHRA PRADESH - 533216'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDHRA PRADESH - 533216';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TELANGALANA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TELANGALANA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'VILLUPUTAM, TAMILNADU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'VILLUPUTAM, TAMILNADU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KOTTAPPURAM'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KOTTAPPURAM';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KALLAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KALLAL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vellore'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vellore';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ariyalur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ariyalur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Salem'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Salem';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Thiruvanamalai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Thiruvanamalai';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kanchipuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kanchipuram';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kallakkurichi'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kallakkurichi';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Perambalur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Perambalur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tirupathur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tirupathur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Villiupuram'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Villiupuram';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAYILADUTHURAI(DT)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAYILADUTHURAI(DT)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'ANDAMAN AND NICOBAR ISLANDS.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'ANDAMAN AND NICOBAR ISLANDS.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Rohtak'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Rohtak';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kureepuzha'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kureepuzha';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Jaunpur'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Jaunpur';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KAPPAKUDI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KAPPAKUDI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nandyal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nandyal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Hoshiarpur,Punjab'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Hoshiarpur,Punjab';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KURNOOL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KURNOOL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhilai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhilai';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Madurai, Tamil Nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Madurai, Tamil Nadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Paramakudi, Tamil Nadu'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Paramakudi, Tamil Nadu';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'vikarabad (dist)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'vikarabad (dist)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAGULAPALLI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAGULAPALLI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Salem , Tamilnadu .'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Salem , Tamilnadu .';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bahadurgarh'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bahadurgarh';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gobichettipalayam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gobichettipalayam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Guntur, Andhra Pradesh.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Guntur, Andhra Pradesh.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TRICHY, TAMILNADU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TRICHY, TAMILNADU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pottammal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pottammal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KRISHNAGIRI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KRISHNAGIRI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARINGANNOOR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARINGANNOOR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TENKASI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TENKASI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Ottapalam Palakkad'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Ottapalam Palakkad';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Edathilambalam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Edathilambalam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vennicode'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vennicode';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Sundarapandiam'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Sundarapandiam';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kerla'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kerla';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Kumarkhal'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Kumarkhal';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Surendranagar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Surendranagar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'HARAYANA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'HARAYANA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KARNATKA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KARNATKA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMIL NDU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMIL NDU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MANNARGUDI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MANNARGUDI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'VASUDEVANALLUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'VASUDEVANALLUR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMIL NADYU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMIL NADYU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TMIL NADU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TMIL NADU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TAMIL NAADU'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TAMIL NAADU';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTAR PRDESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTAR PRDESH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'CHHATISHGARH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'CHHATISHGARH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'THE NILGIRIS'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'THE NILGIRIS';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Tiruppur District'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Tiruppur District';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TIRUCHIRAPPALLI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TIRUCHIRAPPALLI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DEKHI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DEKHI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTARAKHAND'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTARAKHAND';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Aluva'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Aluva';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'kanyakumari'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'kanyakumari';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Pudukottai'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Pudukottai';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NEW DELHI.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NEW DELHI.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PUNJAB.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PUNJAB.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DELHI CANTT'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DELHI CANTT';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RAJASHTAN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RAJASHTAN';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'LUCKNOW, U.P.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'LUCKNOW, U.P.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTRANCHAL'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTRANCHAL';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'FARIDKOT'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'FARIDKOT';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BACHHRAWAN'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BACHHRAWAN';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BAHRAICH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BAHRAICH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'U. T.'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'U. T.';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'PITHORAGARH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'PITHORAGARH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NOIDA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NOIDA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UTTAR PRDAESH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UTTAR PRDAESH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'J & K'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'J & K';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RAJKOT'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RAJKOT';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIU (U.T.)'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIU (U.T.)';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Gaya'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Gaya';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Palta'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Palta';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhavnagar'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhavnagar';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Mehsana'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Mehsana';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'New Panvel East'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'New Panvel East';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KHARGHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KHARGHAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BELAPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BELAPUR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NEW MUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NEW MUMBAI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'DIST.RAIGAD'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'DIST.RAIGAD';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAVI MUMBI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAVI MUMBI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAVI MUUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAVI MUUMBAI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NABI MUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NABI MUMBAI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RAIGARH'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RAIGARH';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NAVI MIUMBAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NAVI MIUMBAI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'RANCHI, C/O 56 APO'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'RANCHI, C/O 56 APO';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'BEGUSARAI'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'BEGUSARAI';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KATIHAR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KATIHAR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KANKARBAG'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KANKARBAG';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'TSEUNG KWAN O.N.T'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'TSEUNG KWAN O.N.T';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'C/O. 56 APO'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'C/O. 56 APO';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'CA 94583'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'CA 94583';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Rajasthan,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Rajasthan,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Goa,'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Goa,';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vasco Goa'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vasco Goa';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NORTH - GOA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NORTH - GOA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UP'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UP';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Salcete'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Salcete';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'NORTHGOA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'NORTHGOA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Nerul'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Nerul';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Vasco'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Vasco';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Betul'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Betul';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Bhayandar East'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Bhayandar East';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'Candolim'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'Candolim';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'SAHARANPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'SAHARANPUR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'KERANA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'KERANA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'UDAIPUR'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'UDAIPUR';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'JEUR,TAL-KARMALA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'JEUR,TAL-KARMALA';
-- 1 rows
UPDATE `address_details` SET `State` = COALESCE(NULLIF(`State`, ''), 'MAHARASHTA'), `Country` = NULL, `UpdatedAt` = @now WHERE `Country` = 'MAHARASHTA';

COMMIT;

-- ============== Summary ==============
-- distinct values fixed:  767
-- total rows fixed:       12188
-- distinct values kept:   62
-- total rows kept:        290
