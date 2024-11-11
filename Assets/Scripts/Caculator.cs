using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
using System.Linq;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI displayText;  // Gán Text component để hiển thị kết quả
    private string input = ""; // Lưu trữ chuỗi phép tính

    [SerializeField] private bool isCalDone = false;

    // Phương thức xử lý khi nhấn các nút số
    public void OnNumberButtonClick(string number)
    {
        if(isCalDone)
        {
            input = "";
            UpdateDisplay(input);
            isCalDone = false;
        }
        input += number;
        UpdateDisplay(input);
    }

    // Method to handle the reciprocal button click (1/x)
    public void OnReciprocalButtonClick()
    {
        if (input.Length > 0)
        {
            try
            {
                var currentNumber = new DataTable().Compute(input, null);
                if (double.Parse(currentNumber.ToString()) == 0)
                {
                    UpdateDisplay("Error");
                }
                else
                {
                    var reciprocalResult = 1 / double.Parse(currentNumber.ToString());
                    input = reciprocalResult.ToString();
                    UpdateDisplay(input);
                }
            }
            catch
            {
                UpdateDisplay("Error");
            }
        }
        isCalDone = true;
    }

    public void OnSquareButtonClick()
    {
        if (input.Length > 0)
        {
            try
            {
                // Compute the square of the current expression
                var currentNumber = new DataTable().Compute(input, null);
                var squaredResult = Mathf.Pow(float.Parse(currentNumber.ToString()), 2);
                input = squaredResult.ToString();
                UpdateDisplay(input);
            }
            catch
            {
                UpdateDisplay("Error");
            }
        }
        isCalDone = true;
    }

    // Method to handle the square root button click (√x)
    public void OnSquareRootButtonClick()
    {
        if (input.Length > 0)
        {
            try
            {
                var currentNumber = new DataTable().Compute(input, null);
                var sqrtResult = Mathf.Sqrt(float.Parse(currentNumber.ToString()));

                if (double.Parse(currentNumber.ToString()) < 0)
                {
                    UpdateDisplay("Error");
                }
                else
                {
                    input = sqrtResult.ToString();
                    UpdateDisplay(input);
                }
            }
            catch
            {
                UpdateDisplay("Error");
            }
        }

        isCalDone = true;
    }

    // Method to handle the decimal button click
    public void OnDecimalButtonClick()
    {
        if (input.Length == 0 || "+-*/".Contains(input[input.Length - 1].ToString()))
        {
            // If there's no number or an operator is the last character, start with "0."
            input += "0.";
        }
        else
        {
            // Only add a decimal if the current number doesn't already have one
            int lastOperatorIndex = input.LastIndexOfAny(new char[] { '+', '-', '*', '/' });
            string currentNumber = lastOperatorIndex != -1 ? input.Substring(lastOperatorIndex + 1) : input;

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
        if (input.Length > 0)
        {
            int lastOperatorIndex = input.LastIndexOfAny(new char[] { '+', '-', '*', '/' });
            string currentNumber = lastOperatorIndex != 0 ? input.Substring(lastOperatorIndex + 1) : input;

            if (currentNumber.StartsWith("-"))
            {
                input = input.Substring(0, lastOperatorIndex) + currentNumber.Substring(1); // Remove the negative sign
            }
            else
            {
                input = input.Substring(0, lastOperatorIndex + 1) + "-" + currentNumber; // Add the negative sign
            }

            UpdateDisplay(input);
        }
    }

    // New Method to handle the percentage button click
    public void OnPercentageButtonClick()
    {
        if (input.Length > 0)
        {
            try
            {
                var currentNumber = new DataTable().Compute(input, null);
                var percentageResult = double.Parse(currentNumber.ToString()) / 100;
                input = percentageResult.ToString();
                UpdateDisplay(input);
            }
            catch
            {
                UpdateDisplay("Error");
            }
        }
    }

    public void OnDeleteLastCharacterClick()
    {
        if (input.Length > 0)
        {
            input = input.Substring(0, input.Length - 1);
            UpdateDisplay(input == "" ? "0" : input);
        }
    }

    public void OnDeleteLastNumberClick()
    {
        if (input.Length > 0)
        {
            // Tìm vị trí của phép toán gần nhất
            int lastOperatorIndex = input.LastIndexOfAny(new char[] { '+', '-', '*', '/' });
            if (lastOperatorIndex != -1)
            {
                // Xóa từ phép toán gần nhất cho đến hết
                input = input.Substring(0, lastOperatorIndex + 1);
            }
            else
            {
                // Nếu không có phép toán nào, xóa toàn bộ
                input = "";
            }
            UpdateDisplay(input == "" ? "0" : input);
        }
    }

    public void OnDelAllButtonClick()
    {
        if(input.Length > 0)
        {
            input = "";

            UpdateDisplay(input == "" ? "0" : input);
        }
    }

    // Phương thức xử lý khi nhấn các nút phép toán
    public void OnOperatorButtonClick(string operatorSign)
    {
        if (input.Length > 0)
        {
            // Kiểm tra nếu ký tự cuối là phép toán, thay thế nó
            if ("+-*/".Contains(input[input.Length - 1].ToString()))
            {
                input = input.Substring(0, input.Length - 1);
            }
            input += operatorSign;
            UpdateDisplay(input);
        }

        isCalDone = false;
    }

    // Phương thức xử lý khi nhấn nút "="
    public void OnEqualsButtonClick()
    {
        try
        {
            // Dùng DataTable để tính toán chuỗi phép tính
            var result = new DataTable().Compute(input, null);
            input = result.ToString();
            UpdateDisplay(input);
        }
        catch
        {
            UpdateDisplay("Error");
        }

        isCalDone = true;
    }

    // Phương thức xử lý khi nhấn nút "C" (clear)
    public void OnClearButtonClick()
    {
        input = "";
        UpdateDisplay("0");
    }

    // Phương thức cập nhật hiển thị
    private void UpdateDisplay(string text)
    {
        displayText.text = text;
    }
}
