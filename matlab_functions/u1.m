utc1 = [18, 8, 20, 11, 50, 0];
utc2 = [18, 8, 20, 11, 50, 59];

%% to MJD
mjd1 = utc2mjd(utc1);
mjd2 = utc2mjd(utc2);

%% to GPST
gpst1 = mjd2gpst(mjd1);
gpst2 = mjd2gpst(mjd2);

%% difference check
origin = utc2(6) - utc1(6);
result = gpst2(2) - gpst1(2);
fprintf ('origin : %d, result : %d, difference : %d', origin, result, abs(origin - result));