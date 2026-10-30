using System;
using System.Collections.Generic;
using System.Diagnostics;

// Chạy benchmark riêng khi được gọi với tùy chọn --benchmark.
if (args.Length > 0 && args[0] == "--benchmark")
{
    RunBenchmark();
}
else
{
    // Đọc chuỗi cần kiểm tra; dùng chuỗi rỗng nếu không có dữ liệu.
    string input = Console.ReadLine() ?? "";

    // Kiểm tra chuỗi và chuyển mã kết quả thành thông báo như chế độ thường.
    Console.WriteLine(FormatResult(CheckBrackets(input)));
}

// Kiểm tra hai trường hợp hiệu năng lớn, chỉ đo thời gian gọi hàm kiểm tra.
static void RunBenchmark()
{
    // Làm nóng hàm một lần trước khi đo để không tính chi phí khởi động ban đầu.
    CheckBrackets("");

    // Tạo dữ liệu benchmark trước khi bắt đầu đo thời gian từng ca.
    string balancedInput = new string('(', 500000) + new string(')', 500000);

    // Tạo dữ liệu benchmark gồm một triệu ngoặc mở trước khi bắt đầu đo.
    string unclosedInput = new string('(', 1000000);

    // Đo ca 9 với một triệu ký tự ngoặc cân bằng.
    RunBenchmarkCase(9, "500000 ngoặc mở + 500000 ngoặc đóng", balancedInput, -1);

    // Đo ca 10 với một triệu ngoặc mở chưa được đóng.
    RunBenchmarkCase(10, "1000000 ngoặc mở chưa đóng", unclosedInput, 0);
}

// Đo riêng lời gọi CheckBrackets rồi hiển thị kết quả ngoài khoảng thời gian đo.
static void RunBenchmarkCase(int caseNumber, string description, string input, int expectedErrorPosition)
{
    // Bắt đầu đo ngay trước lời gọi cần kiểm thử, bao gồm cả việc tạo Stack.
    Stopwatch stopwatch = Stopwatch.StartNew();

    // Chạy thuật toán với dữ liệu đã tạo sẵn trước khi đo.
    int actualErrorPosition = CheckBrackets(input);

    // Dừng đo ngay sau khi thuật toán hoàn tất.
    stopwatch.Stop();

    // Chuyển kết quả mong đợi và thực tế sang đúng định dạng thông báo.
    string expected = FormatResult(expectedErrorPosition);
    string actual = FormatResult(actualErrorPosition);

    // In mô tả ngắn gọn, không in chuỗi dữ liệu lớn.
    Console.WriteLine($"--- Ca {caseNumber} ---");
    Console.WriteLine($"Dữ liệu: {description}");
    Console.WriteLine($"Độ dài: {input.Length}");
    Console.WriteLine($"Kết quả mong đợi: {expected}");
    Console.WriteLine($"Kết quả thực tế: {actual}");
    Console.WriteLine($"Trạng thái: {(actualErrorPosition == expectedErrorPosition ? "PASS" : "FAIL")}");
    Console.WriteLine($"Thời gian: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");
    Console.WriteLine($"Dưới 1000 ms: {(stopwatch.Elapsed.TotalMilliseconds < 1000.0 ? "CÓ" : "KHÔNG")}");
}

// Chuyển vị trí lỗi hoặc mã hợp lệ thành thông báo đầu ra của chương trình.
static string FormatResult(int errorPosition)
{
    // Giá trị -1 biểu thị chuỗi ngoặc hợp lệ.
    return errorPosition == -1 ? "HOP LE" : $"SAI tai vi tri {errorPosition}";
}

// Trả về -1 nếu hợp lệ hoặc vị trí lỗi theo logic Stack hiện tại.
static int CheckBrackets(string input)
{
    // Lưu ngoặc mở cùng vị trí để ghép cặp và báo lỗi.
    var stack = new Stack<(char ch, int pos)>();

    // Xét lần lượt từng ký tự từ trái sang phải, bắt đầu từ vị trí 0.
    for (int i = 0; i < input.Length; i++)
    {
        // Lấy ký tự hiện tại để phân loại ngoặc.
        char c = input[i];

        // Ngoặc mở được lưu lại để chờ ngoặc đóng tương ứng.
        if (c == '(' || c == '[' || c == '{')
        {
            // Ngoặc mở mới nhất nằm trên đỉnh Stack.
            stack.Push((c, i));
        }
        // Chỉ xử lý ngoặc đóng; các ký tự khác được bỏ qua.
        else if (c == ')' || c == ']' || c == '}')
        {
            // Stack rỗng nghĩa là không có ngoặc mở để ghép.
            if (stack.Count == 0)
            {
                // Trả về vị trí ngoặc đóng thừa.
                return i;
            }

            // Lấy và xóa ngoặc mở gần nhất để ghép với ngoặc đóng.
            var opening = stack.Pop();

            // Kiểm tra hai ngoặc có thuộc cùng một cặp hay không.
            bool matched =
                (opening.ch == '(' && c == ')') ||
                (opening.ch == '[' && c == ']') ||
                (opening.ch == '{' && c == '}');

            // Ngoặc đóng không khớp với ngoặc mở gần nhất.
            if (!matched)
            {
                // Trả về vị trí ngoặc đóng sai loại đầu tiên.
                return i;
            }
        }
    }

    // Còn phần tử nghĩa là có ngoặc mở chưa được đóng.
    if (stack.Count > 0)
    {
        // Khởi tạo vị trí nhỏ nhất bằng vị trí trên đỉnh Stack.
        int earliest = stack.Peek().pos;

        // Xét các ngoặc còn lại để tìm ngoặc chưa đóng đứng sớm nhất.
        foreach (var opening in stack)
        {
            // Chỉ cập nhật khi tìm được vị trí nhỏ hơn.
            if (opening.pos < earliest)
            {
                // Ghi nhận vị trí sớm nhất đã tìm được.
                earliest = opening.pos;
            }
        }

        // Trả về ngoặc mở chưa đóng đứng sớm nhất theo yêu cầu đề.
        return earliest;
    }

    // Không gặp lỗi và không còn ngoặc mở chờ đóng.
    return -1;
}
