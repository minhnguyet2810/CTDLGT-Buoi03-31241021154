using System;

// Tạo MinStack để chạy các ca kiểm thử.
var stack = new MinStack();

// Lưu các giá trị cần thêm theo thứ tự đề yêu cầu.
int[] values = { 5, 3, 7, 3, 8 };

// Lưu giá trị nhỏ nhất mong đợi sau mỗi lần Push.
int[] expectedPush = { 5, 3, 3, 3, 3 };

// Đếm số phép kiểm tra thành công.
int passed = 0;

// Đếm tổng số phép kiểm tra đã thực hiện.
int total = 0;

// Bắt đầu kiểm tra Push và GetMin.
Console.WriteLine("=== CA 1: PUSH VA GETMIN ===");

// Xét lần lượt các giá trị cần thêm.
for (int i = 0; i < values.Length; i++)
{
    // Thêm giá trị hiện tại lên đỉnh ngăn xếp.
    stack.Push(values[i]);

    // Lấy giá trị nhỏ nhất sau khi thêm.
    int actual = stack.GetMin();

    // Đối chiếu cả giá trị nhỏ nhất và số phần tử.
    bool correct =
        actual == expectedPush[i] &&
        stack.Count == i + 1;

    // Ghi nhận một phép kiểm tra.
    total++;

    // Chỉ tăng số đạt khi kết quả đúng.
    if (correct)
    {
        // Ghi nhận phép kiểm tra thành công.
        passed++;
    }

    // Hiển thị dữ liệu thêm vào và kết quả đối chiếu.
    Console.WriteLine(
        $"Push {values[i]} | Min mong doi: {expectedPush[i]} | " +
        $"Min thuc te: {actual} | Count: {stack.Count} | " +
        $"{(correct ? "PASS" : "FAIL")}");
}

// Lưu các phần tử mong đợi lấy ra trong bốn lần Pop.
int[] expectedPop = { 8, 3, 7, 3 };

// Lưu giá trị nhỏ nhất mong đợi sau từng lần Pop.
int[] expectedMin = { 3, 3, 3, 5 };

// Bắt đầu kiểm tra Pop kết hợp GetMin.
Console.WriteLine("\n=== CA 2: POP VA GETMIN ===");

// Lấy bốn phần tử theo đúng chuỗi thao tác của đề.
for (int i = 0; i < expectedPop.Length; i++)
{
    // Lấy và xóa phần tử trên đỉnh.
    int removed = stack.Pop();

    // Đọc giá trị nhỏ nhất của ngăn xếp còn lại.
    int actualMin = stack.GetMin();

    // Kiểm tra phần tử lấy ra, min và số phần tử còn lại.
    bool correct =
        removed == expectedPop[i] &&
        actualMin == expectedMin[i] &&
        stack.Count == 4 - i;

    // Ghi nhận thêm một phép kiểm tra.
    total++;

    // Chỉ ghi nhận đạt nếu mọi điều kiện đều đúng.
    if (correct)
    {
        // Tăng số phép kiểm tra thành công.
        passed++;
    }

    // Hiển thị kết quả thực tế và mong đợi của lần Pop.
    Console.WriteLine(
        $"Pop mong doi: {expectedPop[i]} | Pop thuc te: {removed} | " +
        $"Min mong doi: {expectedMin[i]} | Min thuc te: {actualMin} | " +
        $"Count: {stack.Count} | {(correct ? "PASS" : "FAIL")}");
}

// Bắt đầu kiểm tra lấy phần tử cuối để đưa ngăn xếp về rỗng.
Console.WriteLine("\n=== CA 3: LAY PHAN TU CUOI ===");

// Lấy số 5 còn lại sau bốn lần Pop.
int last = stack.Pop();

// Kiểm tra lấy đúng phần tử cuối và Count trở về 0.
bool lastCorrect = last == 5 && stack.Count == 0;

// Ghi nhận phép kiểm tra phần tử cuối.
total++;

// Chỉ ghi nhận đạt khi giá trị và trạng thái đều đúng.
if (lastCorrect)
{
    // Tăng số phép kiểm tra thành công.
    passed++;
}

// Hiển thị trạng thái sau khi lấy hết phần tử.
Console.WriteLine(
    $"Pop mong doi: 5 | Pop thuc te: {last} | " +
    $"Count: {stack.Count} | {(lastCorrect ? "PASS" : "FAIL")}");

// Bắt đầu kiểm tra ngoại lệ trên ngăn xếp rỗng.
Console.WriteLine("\n=== CA 4: NGOAI LE KHI RONG ===");

// Ghi nhận phép kiểm tra GetMin trên ngăn xếp rỗng.
total++;

// Gọi GetMin khi không có phần tử để xác nhận hành vi lỗi.
try
{
    // GetMin phải ném ngoại lệ vì không có phần tử.
    stack.GetMin();

    // Nếu không có ngoại lệ thì phép kiểm tra thất bại.
    Console.WriteLine("GetMin: FAIL - khong nem ngoai le");
}
// Chỉ tính đạt khi GetMin ném đúng InvalidOperationException.
catch (InvalidOperationException)
{
    // Ghi nhận chương trình ném đúng loại ngoại lệ.
    passed++;

    // Hiển thị kết quả kiểm tra GetMin.
    Console.WriteLine("GetMin: InvalidOperationException - PASS");
}

// Ghi nhận phép kiểm tra Pop trên ngăn xếp rỗng.
total++;

// Gọi Pop khi không có phần tử để xác nhận hành vi lỗi.
try
{
    // Pop phải ném ngoại lệ vì không có phần tử để lấy.
    stack.Pop();

    // Nếu không có ngoại lệ thì phép kiểm tra thất bại.
    Console.WriteLine("Pop: FAIL - khong nem ngoai le");
}
// Chỉ tính đạt khi Pop ném đúng InvalidOperationException.
catch (InvalidOperationException)
{
    // Ghi nhận Pop xử lý đúng ngăn xếp rỗng.
    passed++;

    // Hiển thị kết quả kiểm tra Pop.
    Console.WriteLine("Pop: InvalidOperationException - PASS");
}

// Hiển thị số phép kiểm tra thành công trên tổng số.
Console.WriteLine($"\nTong ket: PASS {passed}/{total}");

// Hiển thị số phép kiểm tra thất bại.
Console.WriteLine($"FAIL: {total - passed}");

// Cài đặt MinStack bằng các nút liên kết để mỗi thao tác là O(1).
public class MinStack
{
    // Trỏ đến nút trên đỉnh; null biểu thị ngăn xếp rỗng.
    private Node? top;

    // Lưu số phần tử hiện có để đọc Count trong O(1).
    private int count;

    // Trả số phần tử mà không cần duyệt các nút.
    public int Count => count;

    // Thêm một phần tử mới lên đỉnh ngăn xếp.
    public void Push(int x)
    {
        // Nếu đang rỗng thì x là min; nếu không thì so với min cũ.
        int minimum = top == null
            ? x
            : Math.Min(x, top.Minimum);

        // Tạo nút mới chứa x, min mới và liên kết đến đỉnh cũ.
        top = new Node(x, minimum, top);

        // Ghi nhận ngăn xếp có thêm một phần tử.
        count++;
    }

    // Lấy và xóa phần tử trên đỉnh ngăn xếp.
    public int Pop()
    {
        // Không cho phép lấy phần tử khi ngăn xếp rỗng.
        if (top == null)
        {
            // Báo đúng loại ngoại lệ theo yêu cầu đề.
            throw new InvalidOperationException("Ngan xep rong.");
        }

        // Giữ giá trị đỉnh để trả về sau khi bỏ nút.
        int value = top.Value;

        // Chuyển đỉnh xuống nút phía dưới, đồng thời khôi phục min cũ.
        top = top.Next;

        // Ghi nhận một phần tử đã được lấy ra.
        count--;

        // Trả giá trị của phần tử vừa xóa.
        return value;
    }

    // Xem giá trị nhỏ nhất mà không thay đổi ngăn xếp.
    public int GetMin()
    {
        // Ngăn xếp rỗng không có giá trị nhỏ nhất để trả về.
        if (top == null)
        {
            // Báo đúng loại ngoại lệ theo yêu cầu đề.
            throw new InvalidOperationException("Ngan xep rong.");
        }

        // Min lưu ở đỉnh đại diện cho toàn bộ ngăn xếp hiện tại.
        return top.Minimum;
    }

    // Mỗi nút lưu dữ liệu, min tại độ sâu đó và liên kết xuống dưới.
    private class Node
    {
        // Lưu giá trị của phần tử tại nút này.
        public readonly int Value;

        // Lưu giá trị nhỏ nhất từ nút này đến đáy ngăn xếp.
        public readonly int Minimum;

        // Trỏ đến nút ngay bên dưới; null nếu đây là nút đáy.
        public readonly Node? Next;

        // Khởi tạo một nút với dữ liệu và liên kết đã xác định.
        public Node(int value, int minimum, Node? next)
        {
            // Gán giá trị phần tử cho nút mới.
            Value = value;

            // Ghi nhớ min để GetMin không cần duyệt lại.
            Minimum = minimum;

            // Giữ liên kết đến phần ngăn xếp đã có.
            Next = next;
        }
    }
}