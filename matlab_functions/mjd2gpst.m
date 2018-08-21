function [ gpst ] = mjd2gpst( mjd )
    gpst = [floor((mjd(1) - 44244) / 7), 0];
    remain = mjd(1) - gpst(1) * 7 - 44244;
    gpst(2) = (remain + mjd(2)) * 86400;
end

