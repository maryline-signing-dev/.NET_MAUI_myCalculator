using System;
using System.Globalization;

namespace myCalculator.Services;

public class CalculatorEngine
{
    // ═════════════════════ État de la calculatrice ═════════════════════

    private string _currentInput = "0";

    private double? _firstOperand;

    private string? _operator;

    private bool _shouldResetInput;

    private bool _hasError;


    // ═════════════════════ Propriétés publiques ═════════════════════

    // Texte affiché comme résultat principal.
    public string DisplayText =>
        _hasError ? "Erreur" : _currentInput;

    // Texte affiché au-dessus du résultat.
    public string ExpressionText
    {
        get
        {
            if (_hasError)
                return string.Empty;

            if (_firstOperand is not null && _operator is not null)
                return $"{FormatNumber(_firstOperand.Value)} {_operator}";

            return string.Empty;
        }
    }

    // Indique si la calculatrice est actuellement en erreur.
    public bool HasError => _hasError;


    // ═════════════════════ Saisie des nombres ═════════════════════

    // ───────────── Chiffre ─────────────

    public void InputDigit(char digit)
    {
        if (!char.IsDigit(digit))
            return;

        ClearErrorIfNeeded();

        // Après un opérateur ou un calcul, le prochain chiffre commence un nouveau nombre.
        if (_shouldResetInput)
        {
            _currentInput = digit.ToString();
            _shouldResetInput = false;
            return;
        }

        // Évite d'avoir plusieurs zéros au début.
        if (_currentInput == "0")
        {
            _currentInput = digit.ToString();
            return;
        }

        // Limite la longueur du nombre saisi.
        if (_currentInput.Length >= 15)
            return;

        _currentInput += digit;
    }


    // ───────────── Décimal ─────────────

    public void InputDecimal()
    {
        ClearErrorIfNeeded();

        // Après un opérateur ou un calcul, on commence directement un nouveau nombre décimal.
        if (_shouldResetInput)
        {
            _currentInput = "0.";
            _shouldResetInput = false;
            return;
        }

        // Un seul séparateur décimal par nombre.
        if (_currentInput.Contains('.'))
            return;

        _currentInput += ".";
    }


    // ═════════════════════ Gestion des opérateurs ═════════════════════

    public void SetOperator(string operatorSymbol)
    {
        if (_hasError)
            return;

        if (!IsValidOperator(operatorSymbol))
            return;

        double currentValue = ParseCurrentValue();

        if (_hasError)
            return;

        // Si une opération est déjà en attente,  on la calcule avant de prendre le nouvel opérateur.
        if (_firstOperand is not null &&
            _operator is not null &&
            !_shouldResetInput)
        {
            Calculate();

            if (_hasError)
                return;

            currentValue = ParseCurrentValue();
        }

        _firstOperand = currentValue;
        _operator = operatorSymbol;

        _shouldResetInput = true;
    }


    // ═════════════════════ Calcul ═════════════════════

    public void Calculate()
    {
        if (_hasError)
            return;

        if (_firstOperand is null ||
            _operator is null)
        {
            return;
        }

        double secondOperand = ParseCurrentValue();

        if (_hasError)
            return;

        double result;

        switch (_operator)
        {
            case "+":
                result = _firstOperand.Value + secondOperand;
                break;

            case "−":
                result = _firstOperand.Value - secondOperand;
                break;

            case "×":
                result = _firstOperand.Value * secondOperand;
                break;

            case "÷":

                if (secondOperand == 0)
                {
                    SetError();
                    return;
                }

                result = _firstOperand.Value / secondOperand;
                break;

            case "xʸ":

                result = Math.Pow(_firstOperand.Value, secondOperand);
                break;

            case "ⁿ√x":

                double rootIndex = _firstOperand.Value;
                double radicand = secondOperand;

                if (rootIndex == 0)
                {
                    SetError();
                    return;
                }

                // Pour une racine paire, un nombre négatif
                if (rootIndex % 2 == 0 &&
                    radicand < 0)
                {
                    SetError();
                    return;
                }

                result = Math.Pow(
                    radicand,
                    1.0 / rootIndex);

                break;

            default:
                return;
        }

        _currentInput = FormatNumber(result);

        _firstOperand = null;
        _operator = null;

        _shouldResetInput = true;
    }


    // ═════════════════════ Fonctions spéciales ═════════════════════

    // ───────────── Changement de signe ─────────────

    public void ToggleSign()
    {
        if (_hasError)
            return;

        if (_currentInput == "0")
            return;

        if (_currentInput.StartsWith('-'))
        {
            _currentInput = _currentInput[1..];
        }
        else
        {
            _currentInput = "-" + _currentInput;
        }
    }


    // ───────────── Pourcentage ─────────────

    public void Percent()
    {
        if (_hasError)
            return;

        double value = ParseCurrentValue();

        if (_hasError)
            return;

        value /= 100;

        _currentInput = FormatNumber(value);
    }

    // ───────────── Carré ─────────────

    public void Square()
    {
        if (_hasError)
            return;

        double value = ParseCurrentValue();

        if (_hasError)
            return;

        double result = value * value;

        _currentInput = FormatNumber(result);

        _shouldResetInput = true;
    }

    // ───────────── Racine carrée ─────────────

    public void SquareRoot()
    {
        if (_hasError)
            return;

        double value = ParseCurrentValue();

        if (_hasError)
            return;

        if (value < 0)
        {
            SetError();
            return;
        }

        double result = Math.Sqrt(value);

        _currentInput = FormatNumber(result);

        _shouldResetInput = true;
    }

    // ───────────── Racine n-ième ─────────────

    public void SetRootOperator()
    {
        SetOperator("ⁿ√x");
    }

    // ───────────── Puissance ─────────────

    public void SetPowerOperator()
    {
        SetOperator("xʸ");
    }

    // ───────────── Factorielle ─────────────

    public void Factorial()
    {
        if (_hasError)
            return;

        double value = ParseCurrentValue();

        if (_hasError)
            return;

        if (value < 0 ||
            value != Math.Truncate(value))
        {
            SetError();
            return;
        }

        // Limite pratique pour éviter un résultat beaucoup trop grand pour un double.
        if (value > 170)
        {
            SetError();
            return;
        }

        double result = 1;

        for (int i = 2; i <= value; i++)
        {
            result *= i;
        }

        _currentInput = FormatNumber(result);

        _shouldResetInput = true;
    }

    // ───────────── Retour arrière ─────────────

    public void Backspace()
    {
        if (_hasError)
        {
            Clear();
            return;
        }

        // Si le résultat vient d'être calculé,le retour arrière recommence sur 0.
        if (_shouldResetInput)
        {
            _currentInput = "0";
            _shouldResetInput = false;
            return;
        }

        if (_currentInput.Length <= 1 ||
            (_currentInput.Length == 2 &&
             _currentInput.StartsWith('-')))
        {
            _currentInput = "0";
            return;
        }

        _currentInput = _currentInput[..^1];

        if (_currentInput == "-" ||
            string.IsNullOrEmpty(_currentInput))
        {
            _currentInput = "0";
        }
    }


    // ───────────── Tout effacer ─────────────

    public void Clear()
    {
        _currentInput = "0";

        _firstOperand = null;
        _operator = null;

        _shouldResetInput = false;
        _hasError = false;
    }


    // ═════════════════════ Gestion des erreurs ═════════════════════

    private void SetError()
    {
        _hasError = true;

        _currentInput = "Erreur";

        _firstOperand = null;
        _operator = null;

        _shouldResetInput = true;
    }


    private void ClearErrorIfNeeded()
    {
        if (!_hasError)
            return;

        Clear();
    }


    // ═════════════════════ Méthodes utilitaires ═════════════════════

    // ───────────── Vérification de l'opérateur ─────────────

    private static bool IsValidOperator(string operatorSymbol)
    {
        return operatorSymbol is
            "+" or
            "−" or
            "×" or
            "÷" or
            "xʸ" or
            "ⁿ√x"; 
    }


    // ───────────── Conversion du nombre saisi ─────────────

    private double ParseCurrentValue()
    {
        if (_currentInput == "Erreur")
        {
            SetError();
            return 0;
        }

        if (double.TryParse(
                _currentInput,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double value))
        {
            return value;
        }

        SetError();

        return 0;
    }


    // ───────────── Formatage des résultats ─────────────

    private static string FormatNumber(double value)
    {
        if (double.IsNaN(value) ||
            double.IsInfinity(value))
        {
            return "Erreur";
        }

        // Évite l'affichage inutile de .0
        // et limite les erreurs d'affichage liées aux doubles.
        return value.ToString(
            "G15",
            CultureInfo.InvariantCulture);
    }
}