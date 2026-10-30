using System;
using System.Diagnostics;

// Ca 1 kiểm tra trạng thái ban đầu và thao tác trên hàng rỗng.
Console.WriteLine("=== CA 1: HANG RONG ===");

// Tạo hàng mới với dung lượng ban đầu bằng 4.
var q1 = new CircularQueue<int>();

// Hiển thị số phần tử và dung lượng ban đầu.
Console.WriteLine($"Count: {q1.Count}, Capacity: {q1.Capacity}");

try
{
    // Thử lấy phần tử từ hàng rỗng để kiểm tra ngoại lệ.
    q1.Dequeue();

    // Nếu không ném ngoại lệ thì thao tác chưa đúng yêu cầu.
    Console.WriteLine("Dequeue: FAIL - khong nem ngoai le");
}
catch (InvalidOperationException)
{
    // Xác nhận Dequeue từ chối lấy phần tử khi hàng rỗng.
    Console.WriteLine("Dequeue: InvalidOperationException - PASS");
}

try
{
    // Thử xem phần tử đầu khi hàng chưa có dữ liệu.
    q1.Peek();

    // Nếu không ném ngoại lệ thì thao tác chưa đúng yêu cầu.
    Console.WriteLine("Peek: FAIL - khong nem ngoai le");
}
catch (InvalidOperationException)
{
    // Xác nhận Peek từ chối đọc phần tử khi hàng rỗng.
    Console.WriteLine("Peek: InvalidOperationException - PASS");
}

// Ca 2 kiểm tra hàng vừa đầy nhưng chưa cần mở rộng.
Console.WriteLine("\n=== CA 2: VUA DAY ===");

// Tạo hàng độc lập để ca trước không ảnh hưởng ca này.
var q2 = new CircularQueue<string>();

// Thêm A làm phần tử đầu hàng.
q2.Enqueue("A");

// Thêm B sau A.
q2.Enqueue("B");

// Thêm C sau B.
q2.Enqueue("C");

// Thêm D để sử dụng hết 4 ô ban đầu.
q2.Enqueue("D");

// Peek phải trả A mà không xóa, Count và Capacity vẫn bằng 4.
Console.WriteLine(
    $"Peek: {q2.Peek()}, Count: {q2.Count}, Capacity: {q2.Capacity}");

// Ca 3 kiểm tra tái sử dụng ô trống bằng cách quay vòng chỉ số.
Console.WriteLine("\n=== CA 3: QUAY VONG ===");

// Tạo hàng mới để kiểm tra quay vòng trong mảng dung lượng 4.
var q3 = new CircularQueue<string>();

// Đưa A vào đầu hàng.
q3.Enqueue("A");

// Đưa B vào sau A.
q3.Enqueue("B");

// Đưa C vào sau B.
q3.Enqueue("C");

// Đưa D vào để hàng đầy.
q3.Enqueue("D");

// Lấy A ra để giải phóng ô đầu tiên.
Console.WriteLine($"Lay ra: {q3.Dequeue()}");

// Lấy B ra để giải phóng thêm một ô.
Console.WriteLine($"Lay ra: {q3.Dequeue()}");

// Thêm E vào một ô trống ở đầu mảng.
q3.Enqueue("E");

// Thêm F vào ô trống tiếp theo mà không mở rộng.
q3.Enqueue("F");

// Lấy hết phần tử còn lại để kiểm tra thứ tự C, D, E, F.
while (q3.Count > 0)
{
    // Mỗi lần Dequeue phải trả phần tử đầu theo thứ tự FIFO.
    Console.WriteLine($"Lay ra: {q3.Dequeue()}");
}

// Dung lượng phải giữ nguyên vì chưa thêm vượt quá 4 phần tử.
Console.WriteLine($"Capacity: {q3.Capacity}");

// Ca 4 kiểm tra mở rộng khi phần tử đầu đang ở chỉ số 0.
Console.WriteLine("\n=== CA 4: MO RONG KHI FRONT = 0 ===");

// Tạo hàng số nguyên với dung lượng ban đầu bằng 4.
var q4 = new CircularQueue<int>();

// Thêm 5 phần tử để buộc hàng mở rộng từ 4 lên 8.
for (int i = 1; i <= 5; i++)
{
    // Giữ thứ tự tăng dần để dễ kiểm tra FIFO.
    q4.Enqueue(i);
}

// Dung lượng sau lần thêm thứ 5 phải bằng 8.
Console.WriteLine($"Capacity: {q4.Capacity}");

// Lấy hết phần tử để kiểm tra thứ tự 1, 2, 3, 4, 5.
while (q4.Count > 0)
{
    // In phần tử thực tế lấy được từ đầu hàng.
    Console.WriteLine($"Lay ra: {q4.Dequeue()}");
}

// Ca 5 kiểm tra mở rộng khi dữ liệu đã quay vòng.
Console.WriteLine("\n=== CA 5: MO RONG KHI FRONT KHAC 0 ===");

// Tạo hàng riêng để kiểm tra sao chép theo thứ tự logic.
var q5 = new CircularQueue<string>();

// Thêm A ở đầu hàng.
q5.Enqueue("A");

// Thêm B sau A.
q5.Enqueue("B");

// Thêm C sau B.
q5.Enqueue("C");

// Thêm D để hàng đầy.
q5.Enqueue("D");

// Lấy A ra để front dịch khỏi chỉ số 0.
Console.WriteLine($"Lay ra truoc mo rong: {q5.Dequeue()}");

// Lấy B ra để front chuyển đến phần tử C.
Console.WriteLine($"Lay ra truoc mo rong: {q5.Dequeue()}");

// Thêm E vào ô trống ở đầu mảng.
q5.Enqueue("E");

// Thêm F để hàng đầy trở lại trong trạng thái quay vòng.
q5.Enqueue("F");

// Thêm G khiến mảng phải mở rộng và sắp lại C, D, E, F.
q5.Enqueue("G");

// Dung lượng sau mở rộng phải bằng 8.
Console.WriteLine($"Capacity: {q5.Capacity}");

// Lấy hết phần tử để xác nhận thứ tự C, D, E, F, G.
while (q5.Count > 0)
{
    // In kết quả thực tế sau khi mở rộng mảng.
    Console.WriteLine($"Lay ra: {q5.Dequeue()}");
}

// Ca 6 kiểm tra quay vòng nhiều lần mà không tăng dung lượng.
Console.WriteLine("\n=== CA 6: MOT TRIEU LAN THEM ROI LAY ===");

// Tạo hàng luôn có nhiều nhất một phần tử.
var q6 = new CircularQueue<int>();

// Ghi nhận xem mọi phần tử lấy ra có đúng hay không.
bool correct6 = true;

// Lặp một triệu lần để kiểm tra việc quay vòng chỉ số.
for (int i = 0; i < 1_000_000; i++)
{
    // Thêm giá trị hiện tại vào hàng.
    q6.Enqueue(i);

    // Lấy ngay ra và đối chiếu với giá trị vừa thêm.
    if (q6.Dequeue() != i)
    {
        // Ghi nhận lỗi nếu lấy ra sai phần tử.
        correct6 = false;
    }
}

// Kiểm tra cả dữ liệu, trạng thái rỗng và dung lượng không đổi.
Console.WriteLine(
    $"Ket qua: {(correct6 && q6.Count == 0 && q6.Capacity == 4 ? "PASS" : "FAIL")}");

// Hiển thị trạng thái cuối để làm minh chứng.
Console.WriteLine($"Count: {q6.Count}, Capacity: {q6.Capacity}");

// Ca 7 kiểm tra nhiều lần mở rộng và đo thời gian xử lý.
Console.WriteLine("\n=== CA 7: THEM 100000 PHAN TU ROI LAY HET ===");

// Tạo hàng riêng để khởi động các thao tác trước khi đo.
var warmup = new CircularQueue<int>();

// Chạy thử thao tác thêm để khởi động mã thực thi.
for (int i = 0; i < 1000; i++)
{
    // Thêm dữ liệu vào hàng khởi động.
    warmup.Enqueue(i);
}

// Chạy thử thao tác lấy trên hàng khởi động.
while (warmup.Count > 0)
{
    // Loại bỏ dữ liệu khởi động, không dùng cho kết quả đo.
    warmup.Dequeue();
}

// Tạo đồng hồ trước khoảng đo.
var timer = new Stopwatch();

// Bắt đầu đo cả việc tạo hàng, thêm, lấy và đối chiếu dữ liệu.
timer.Start();

// Tạo hàng mới với dung lượng 4 cho ca đo chính thức.
var q7 = new CircularQueue<int>();

// Ghi nhận kết quả kiểm tra thứ tự lấy ra.
bool correct7 = true;

// Thêm 100000 số nguyên theo thứ tự tăng dần.
for (int i = 0; i < 100_000; i++)
{
    // Mỗi lần hàng đầy, Enqueue sẽ tự mở rộng.
    q7.Enqueue(i);
}

// Lấy đủ 100000 phần tử để kiểm tra FIFO.
for (int i = 0; i < 100_000; i++)
{
    // Giá trị lấy ra phải trùng với thứ tự đã thêm.
    if (q7.Dequeue() != i)
    {
        // Ghi nhận lỗi nếu thứ tự không còn đúng.
        correct7 = false;
    }
}

// Dừng đo trước khi in dữ liệu ra Terminal.
timer.Stop();

// Kiểm tra thứ tự, trạng thái rỗng và dung lượng cuối theo đề.
Console.WriteLine(
    $"Ket qua: {(correct7 && q7.Count == 0 && q7.Capacity == 131072 ? "PASS" : "FAIL")}");

// Dung lượng giữ nguyên sau khi lấy hết vì lớp không tự thu nhỏ.
Console.WriteLine($"Count: {q7.Count}, Capacity: {q7.Capacity}");

// In số đo thực tế, không bao gồm thời gian xuất kết quả.
Console.WriteLine($"Thoi gian: {timer.Elapsed.TotalMilliseconds:F3} ms");

// Cài đặt hàng đợi FIFO bằng mảng có chỉ số quay vòng.
public class CircularQueue<T>
{
    // Mảng lưu phần tử, ban đầu có 4 ô theo yêu cầu đề.
    private T[] items = new T[4];

    // Chỉ số phần tử đầu hàng, cũng là vị trí lấy ra tiếp theo.
    private int front = 0;

    // Chỉ số ô sẽ nhận phần tử mới ở lần Enqueue tiếp theo.
    private int rear = 0;

    // Số phần tử thực tế để phân biệt hàng rỗng và hàng đầy.
    private int count = 0;

    // Trả số phần tử hiện có mà không cần duyệt mảng.
    public int Count => count;

    // Trả dung lượng hiện tại của mảng lưu trữ.
    public int Capacity => items.Length;

    // Thêm phần tử vào cuối hàng và mở rộng khi cần.
    public void Enqueue(T value)
    {
        // Chỉ mở rộng khi không còn ô trống để thêm.
        if (count == items.Length)
        {
            // Tăng dung lượng và giữ nguyên thứ tự FIFO.
            Grow();
        }

        // Ghi phần tử mới vào ô cuối hàng đang chờ sử dụng.
        items[rear] = value;

        // Chuyển rear sang ô tiếp theo, quay về 0 nếu hết mảng.
        rear = (rear + 1) % items.Length;

        // Ghi nhận hàng vừa có thêm một phần tử.
        count++;
    }

    // Lấy và xóa phần tử đầu mà không dồn các phần tử còn lại.
    public T Dequeue()
    {
        // Ngăn lấy dữ liệu khi hàng không có phần tử.
        if (count == 0)
        {
            // Báo lỗi theo đúng loại ngoại lệ đề yêu cầu.
            throw new InvalidOperationException("Hang doi rong.");
        }

        // Giữ lại phần tử đầu để trả về sau khi cập nhật hàng.
        T value = items[front];

        // Xóa tham chiếu cũ để tránh giữ đối tượng không còn trong hàng.
        items[front] = default!;

        // Dịch đầu hàng sang ô kế tiếp theo vòng tròn.
        front = (front + 1) % items.Length;

        // Ghi nhận một phần tử vừa được lấy ra.
        count--;

        // Trả phần tử đã lưu trước khi xóa ô.
        return value;
    }

    // Xem phần tử đầu mà không thay đổi hàng.
    public T Peek()
    {
        // Không cho phép đọc phần tử đầu khi hàng rỗng.
        if (count == 0)
        {
            // Dùng cùng loại ngoại lệ như Dequeue trên hàng rỗng.
            throw new InvalidOperationException("Hang doi rong.");
        }

        // Đọc trực tiếp phần tử đầu, giữ nguyên front và count.
        return items[front];
    }

    // Tăng gấp đôi dung lượng khi mảng hiện tại đã đầy.
    private void Grow()
    {
        // Cấp phát mảng mới có dung lượng gấp đôi mảng cũ.
        T[] newItems = new T[items.Length * 2];

        // Chép từng phần tử theo thứ tự từ đầu đến cuối hàng.
        for (int i = 0; i < count; i++)
        {
            // Quy đổi vị trí logic i sang vị trí thực trong mảng cũ.
            newItems[i] = items[(front + i) % items.Length];
        }

        // Chuyển sang dùng mảng mới đã chứa đúng thứ tự FIFO.
        items = newItems;

        // Sau khi chép, phần tử đầu nằm tại ô 0.
        front = 0;

        // Ô trống tiếp theo nằm ngay sau count phần tử đã chép.
        rear = count;
    }
}