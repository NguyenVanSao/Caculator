using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
using System.Linq;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI displayText;  // Gán Text component để hiển thị kết quả

    private const string ERROR_MESSAGE = "Error";
    private const string DEFAULT_DISPLAY = "0";
    private const string OPERATORS = "+-*/";
    
    private string input = ""; // Lưu trữ chuỗi phép tính

    private bool isCalDone = false;

    private bool HasInput => input.Length > 0;

    private void UpdateDisplay(string text)
    {
        displayText.text = text;
    }

    private void ResetInputIfCalculationDone()
    {
        if (isCalDone)
        {
            input = "";
            UpdateDisplay(input);
            isCalDone = false;
        }
    }

    private bool TryEvaluateExpression(out object result)
    {
        try
        {
            result = new DataTable().Compute(input, null);
            return true;
        }
        catch
        {
            result = null;
            UpdateDisplay(ERROR_MESSAGE);
            return false;
        }
    }

    private void ProcessMathOperation(System.Func<double, double> operation)
    {
        if (!HasInput) return;

        if (TryEvaluateExpression(out object result))
        {
            try
            {
                double currentValue = double.Parse(result.ToString());
                double newResult = operation(currentValue);
                input = newResult.ToString();
                UpdateDisplay(input);
                isCalDone = true;
            }
            catch
            {
                UpdateDisplay(ERROR_MESSAGE);
            }
        }
    }

    // Phương thức xử lý khi nhấn các nút số
    public void OnNumberButtonClick(string number)
    {
        ResetInputIfCalculationDone();
        input += number;
        UpdateDisplay(input);
    }

    // Method to handle the reciprocal button click (1/x)
    public void OnReciprocalButtonClick()
    {
        ProcessMathOperation(x => 
        {
            if (x == 0) throw new System.DivideByZeroException();
            return 1 / x;
        });
    }

    public void OnSquareButtonClick()
    {
        ProcessMathOperation(x => Mathf.Pow(x, 2));
    }

    // Method to handle the square root button click (√x)
    public void OnSquareRootButtonClick()
    {
        ProcessMathOperation(x =>
        {
            if (x < 0) throw new System.ArgumentException("Cannot calculate square root of negative number");
            return Mathf.Sqrt((float)x);
        });
    }

    // Method to handle the decimal button click
    public void OnDecimalButtonClick()
    {
        if (!HasInput || OPERATORS.Contains(input[^1]))
        {
            input += "0.";
        }
        else
        {
            int lastOperatorIndex = input.LastIndexOfAny(OPERATORS.ToCharArray());
            string currentNumber = lastOperatorIndex != -1 ? input[(lastOperatorIndex + 1)..] : input;

            if (!currentNumber.Contains("."))
            {
                input += ".";
            }
        }
        UpdateDisplay(input);
    }

    // New Method to handle the +/- toggle button click
    public void OnToggleSignButtonClick()
    {
        if (!HasInput) return;

        int lastOperatorIndex = input.LastIndexOfAny(OPERATORS.ToCharArray());
        string currentNumber = lastOperatorIndex >= 0 ? input[(lastOperatorIndex + 1)..] : input;
        
        if (currentNumber.StartsWith("-"))
        {
            input = input[..(input.Length - currentNumber.Length)] + currentNumber[1..];
        }
        else
        {
            input = input[..(input.Length - currentNumber.Length)] + "-" + currentNumber;
        }
        
        UpdateDisplay(input);
    }

    // New Method to handle the percentage button click
    public void OnPercentageButtonClick()
    {
        ProcessMathOperation(x => x / 100);
    }

    public void OnDeleteLastCharacterClick()
    {
        if (!HasInput) return;
        
        input = input[..^1];
        UpdateDisplay(string.IsNullOrEmpty(input) ? DEFAULT_DISPLAY : input);
    }

    public void OnDeleteLastNumberClick()
    {
        if (!HasInput) return;

        int lastOperatorIndex = input.LastIndexOfAny(OPERATORS.ToCharArray());
        input = lastOperatorIndex != -1 ? input[..(lastOperatorIndex + 1)] : "";
        UpdateDisplay(string.IsNullOrEmpty(input) ? DEFAULT_DISPLAY : input);
    }

    public void OnDelAllButtonClick()
    {
        if (!HasInput) return;
        
        input = "";
        UpdateDisplay(DEFAULT_DISPLAY);
    }

    // Phương thức xử lý khi nhấn các nút phép toán
    public void OnOperatorButtonClick(string operatorSign)
    {
        if (!HasInput) return;

        if (OPERATORS.Contains(input[^1]))
        {
            input = input[..^1];
        }
        input += operatorSign;
        UpdateDisplay(input);
        isCalDone = false;
    }

    // Phương thức xử lý khi nhấn nút "="
    public void OnEqualsButtonClick()
    {
        if (TryEvaluateExpression(out object result))
        {
            input = result.ToString();
            UpdateDisplay(input);
            isCalDone = true;
        }
    }

    // Phương thức xử lý khi nhấn nút "C" (clear)
    public void OnClearButtonClick()
    {
        input = "";
        UpdateDisplay(DEFAULT_DISPLAY);
    }
}
