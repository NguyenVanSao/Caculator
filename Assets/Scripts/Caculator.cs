using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
using System.Linq;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI displayText;  // Gán Text component để hiển thị kết quả
    private string input = ""; // Lưu trữ chuỗi phép tính

    private bool isCalDone = false;

    // Phương thức xử lý khi nhấn các nút số
    public void OnNumberButtonClick(string number)
    {
        if(isCalDone)
        {
            UpdateDisplay("");
            isCalDone = false;
        }
        input += number;
        UpdateDisplay(input);
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
