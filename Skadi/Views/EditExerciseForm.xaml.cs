using Skadi.ViewModels;

namespace Skadi.Views;

public partial class EditExerciseForm : ContentPage
{
	public EditExerciseForm()
	{
		InitializeComponent();
	}

    private void Switch_OnToggled(object? sender, ToggledEventArgs e)
    {
        var vm = BindingContext as EditExerciseFormViewModel;
        if (vm != null)
            vm.DurationOrRepetitionsToggle();
    }
}