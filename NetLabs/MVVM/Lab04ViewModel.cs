using FloxelLib;
using FloxelLib.Common;
using FloxelLib.MVVM;
using NetLabs.Labs.Lab04;
using System;
using System.Globalization;

namespace NetLabs.MVVM;

public sealed partial class Lab04ViewModel : BaseViewModel
{
	private string _firstEquation = "", _secondEquation = "", _thirdEquation = "", _fourthEquation;
	private double _firstError, _secondError, _thirdError, _fourthError;
	private string _aValue = "5", _bValue = "10";

	public string FirstEquation { get => _firstEquation; set => SetField(ref _firstEquation, value); }
	public string SecondEquation { get => _secondEquation; set => SetField(ref _secondEquation, value); }
	public string ThirdEquation { get => _thirdEquation; set => SetField(ref _thirdEquation, value); }
	public string FourthEquation { get => _fourthEquation; set => SetField(ref _fourthEquation, value); }

	public double FirstError { get => _firstError; set => SetField(ref _firstError, value); }
	public double SecondError { get => _secondError; set => SetField(ref _secondError, value); }
	public double ThirdError { get => _thirdError; set => SetField(ref _thirdError, value); }
	public double FourthError { get => _fourthError; set => SetField(ref _fourthError, value); }

	public string AValue
	{
		get => _aValue;
		set
		{
			SetField(ref _aValue, value);
			UpdateValues();
		}
	}

	public string BValue
	{
		get => _bValue;
		set
		{
			SetField(ref _bValue, value);
			UpdateValues();
		}
	}

	public void UpdateValues()
	{
		if (!AValue.Parse(out double a) || !BValue.Parse(out double b)) return;

		int n = 1000000;

		double value1 = Delegates.DefinedIntegral(Delegates.FirstIntegral, a, b);
		double value1aprox = Delegates.RiemannSum(Delegates.FirstFunction, a, b, n);
		FirstError = value1aprox - value1;
		FirstEquation = $"\\int_{{{_aValue}}}^{{{_bValue}}}\\frac{{1}}{{\\sqrt[3]{{x}}}}dx={value1.ToString(CultureInfo.InvariantCulture)}\\approx {value1aprox.ToString(CultureInfo.InvariantCulture)}";

		double value2 = Delegates.DefinedIntegral(Delegates.SecondIntegral, a, b);
		double value2aprox = Delegates.RiemannSum(Delegates.SecondFunction, a, b, n);
		SecondError = value2aprox - value2;
		SecondEquation = $"\\int_{{{_aValue}}}^{{{_bValue}}}\\frac{{sin(x)}}{{\\sqrt{{x^2}}}}dx={value2.ToString(CultureInfo.InvariantCulture)}\\approx {value2aprox.ToString(CultureInfo.InvariantCulture)}";

		double value3 = Delegates.DefinedIntegral(Delegates.ThirdIntegral, a, b);
		double value3aprox = Delegates.RiemannSum(Delegates.ThirdFunction, a, b, n);
		ThirdError = value3aprox - value3;
		ThirdEquation = $"\\int_{{{_aValue}}}^{{{_bValue}}}xcos(x)dx={value3.ToString(CultureInfo.InvariantCulture)}\\approx {value3aprox.ToString(CultureInfo.InvariantCulture)}";

		double value4 = Delegates.DefinedIntegral(Delegates.FourthIntegral, a, b);
		double value4aprox = Delegates.RiemannSum(Delegates.FourthFunction, a, b, n);
		FourthError = value4aprox - value4;
		FourthEquation = $"\\int_{{{_aValue}}}^{{{_bValue}}}x^2dx={value4.ToString(CultureInfo.InvariantCulture)}\\approx {value4aprox.ToString(CultureInfo.InvariantCulture)}";
	}

	public event EventHandler OnKeyPressed;

	public Lab04ViewModel()
	{
		UpdateValues();

		OnKeyPressed += (sender, e) => MessageBox.Show("Олександр", "Делегати", System.Windows.MessageBoxImage.Asterisk);
	}

	[RelayCommand]
	private void KeyDown()
	{
		OnKeyPressed?.Invoke(this, new EventArgs());
	}
}