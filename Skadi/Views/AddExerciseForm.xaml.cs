using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skadi.ViewModels;

namespace Skadi.Views;

public partial class AddExerciseForm : ContentPage
{
    public AddExerciseForm(int workoutId)
    {
        InitializeComponent();
        var vm = BindingContext as AddExerciseFormViewModel;
        if (vm is not null)
            vm.WorkoutId = workoutId;
    }

    private void Switch_OnToggled(object? sender, ToggledEventArgs e)
    {
        var vm = BindingContext as AddExerciseFormViewModel;
        if (vm != null)
            vm.DurationOrRepetitionsToggle();
    }
}