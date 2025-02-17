using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skadi.Models;

namespace Skadi.ViewModels
{
    public partial class WorkoutPageViewModel : ObservableObject
    {
        public Workout Workout { get; set; }

        [ObservableProperty] public string _workoutName;

        public WorkoutPageViewModel()
        {
           
        }
    }
}
