using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skadi.Custom;

public partial class CircularProgressBar : ContentView
{
    private readonly CircularProgressBarDrawable _drawable;
    
    public CircularProgressBar()
    {
        InitializeComponent();
        BindingContext = this; 
        _drawable = new CircularProgressBarDrawable();
        graphicsView.Drawable = _drawable;

        // Manually bind properties
        graphicsView.PropertyChanged += (s, e) => 
        {
            _drawable.Progress = Progress;
            _drawable.Size = Size;
            _drawable.TextColor = TextColor;
            _drawable.Thickness = Thickness;
            _drawable.ProgressColor = ProgressColor;
            _drawable.ProgressLeftColor = ProgressLeftColor;

            graphicsView.Invalidate(); // Force redraw
        };
    }
    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(int), typeof(CircularProgressBar));
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(int), typeof(CircularProgressBar));
    public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(nameof(Thickness), typeof(int), typeof(CircularProgressBar));
    public static readonly BindableProperty ProgressColorProperty = BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircularProgressBar));
    public static readonly BindableProperty ProgressLeftColorProperty = BindableProperty.Create(nameof(ProgressLeftColor), typeof(Color), typeof(CircularProgressBar));
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircularProgressBar));

    public int Progress
    {
        get { return (int)GetValue(ProgressProperty); }
        set { SetValue(ProgressProperty, value); }
    }

    public int Size
    {
        get { return (int)GetValue(SizeProperty); }
        set { SetValue(SizeProperty, value); }
    }

    public int Thickness
    {
        get { return (int)GetValue(ThicknessProperty); }
        set { SetValue(ThicknessProperty, value); }
    }

    public Color ProgressColor
    {
        get { return (Color)GetValue(ProgressColorProperty); }
        set { SetValue(ProgressColorProperty, value); }
    }

    public Color ProgressLeftColor
    {
        get { return (Color)GetValue(ProgressLeftColorProperty); }
        set { SetValue(ProgressLeftColorProperty, value); }
    }

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == SizeProperty.PropertyName)
        {
            HeightRequest = Size;
            WidthRequest = Size;
        }
    }    
}