namespace Skadi.Helpers;

public class TimeHelper
{
    public static int TimeToProgress(int currentMinutes, int currentSeconds, int originalMinutes, int originalSeconds)
    {
        originalSeconds += (originalMinutes * 60);
        currentSeconds += (currentMinutes * 60);
        return (currentSeconds / originalSeconds) * 100;
    }
}