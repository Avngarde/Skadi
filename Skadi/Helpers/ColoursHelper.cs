using Skadi.Models;

namespace Skadi.Helpers
{
    public class ColoursHelper
    {
        public static Color GetExerciseTypeColor(ExerciseType exerciseType)
        {
            Color exerciseTextColor = new Color();

            switch (exerciseType)
            {
                case ExerciseType.Cardio:
                    exerciseTextColor = Color.FromArgb("04E824");
                    break;
                case ExerciseType.Plyometrics:
                    exerciseTextColor = Color.FromArgb("9D96B8");
                    break;
                case ExerciseType.Technique:
                    exerciseTextColor = Color.FromArgb("119DA4");
                    break;
                case ExerciseType.Strength:
                    exerciseTextColor = Color.FromArgb("FB3640");
                    break;
                case ExerciseType.Stretching:
                    exerciseTextColor = Color.FromArgb("937D64");
                    break;
                case ExerciseType.Warmup:
                    exerciseTextColor = Color.FromArgb("96C5F7");
                    break;
            }

            return exerciseTextColor;
        }

        public static (Color, Color) GetWorkoutColours(Difficulty difficulty)
        {
            Color colorBackground;
            Color colorText;
            if (difficulty == Difficulty.Hard)
            {
                colorBackground = Color.FromRgba(255, 230, 226, 255);
                colorText = Color.FromRgba(230, 166, 160, 255);
            }
            else if (difficulty == Difficulty.Medium)
            {
                colorBackground = Color.FromRgba(254, 248, 221, 255);
                colorText = Color.FromRgba(227, 180, 99, 255);
            }
            else
            {
                colorBackground = Color.FromRgba(219, 254, 227, 255);
                colorText = Color.FromRgba(80, 202, 83, 255);
            }

            return (colorBackground, colorText);
        }
    }
}
