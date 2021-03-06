function [ mjd ] = utc2mjd( utc )
    mjd = zeros(2,1);
    y = utc(1);
    if (y < 80)
       y = y + 2000; 
    else
        y = y + 1900;
    end
    m = utc(2);
    if(m <= 2)
        y = y - 1;
        m = m + 12;
    end
        temp = floor(365.25 * y);
        temp = temp + floor(30.6001 * (m + 1));
        temp = temp + utc(3);
        temp = temp - 679019;
        
        mjd(1) = temp;
        mjd(2) = utc(4) + utc(5) / 60 + utc(6) / 3600;
        mjd(2) = mjd(2) / 24;
end

