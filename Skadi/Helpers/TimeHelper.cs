namespace Skadi.Helpers;

public class TimeHelper
{
    public static int TimeToProgress(int currentMinutes, int currentSeconds, int originalMinutes, int originalSeconds)
    {
        originalSeconds += (originalMinutes * 60);
        currentSeconds += (currentMinutes * 60);
        double progressPercent = (double)currentSeconds / (double)originalSeconds;
        int percentInteger = (int)(progressPercent * 100.0);
        return percentInteger;
    }
}
