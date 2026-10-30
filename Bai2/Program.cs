using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

// Chạy chế độ benchmark khi được gọi với tùy chọn --benchmark.
if (args.Length > 0 && args[0] == "--benchmark")
{
    RunBenchmark();
}
else
{
    // Đọc số khách n và số quầy k từ dòng đầu tiên.
    string[] firstLine = Console.ReadLine()!
        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

    // Lưu số khách cần phục vụ.
    int n = int.Parse(firstLine[0]);

    // Lưu số quầy có thể phục vụ đồng thời.
    int k = int.Parse(firstLine[1]);

    // Giữ khách theo thứ tự đến để xử lý đúng nguyên tắc FIFO.
    var customers = new Queue<Customer>();

    // Đọc lần lượt thông tin của n khách.
    for (int i = 1; i <= n; i++)
    {
        // Tách thời điểm đến và thời gian phục vụ của khách hiện tại.
        string[] data = Console.ReadLine()!
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Dùng long để tính toán thời gian thống nhất và tránh tràn số.
        long arrival = long.Parse(data[0]);

        // Lưu thời gian cần phục vụ khách.
        long service = long.Parse(data[1]);

        // Thêm khách vào cuối hàng, giữ nguyên thứ tự nhập.
        customers.Enqueue(new Customer(i, arrival, service));
    }

    // Chạy mô phỏng với đầu ra đầy đủ cho chế độ stdin thông thường.
    SimulationResult result = Simulate(customers, k, printCustomers: true, measure: false);

    // In ba dòng thống kê theo đúng định dạng hiện tại.
    PrintStatistics(result);
}

// Chạy benchmark với dữ liệu chuẩn bị sẵn trước khi bắt đầu đo.
static void RunBenchmark()
{
    // Tạo hàng khởi động riêng để không làm thay đổi hàng sẽ được đo.
    var warmupCustomers = new Queue<Customer>();
    warmupCustomers.Enqueue(new Customer(1, 0, 1));

    // Làm nóng mô phỏng trước khi tạo và đo hàng dữ liệu lớn.
    Simulate(warmupCustomers, 1, printCustomers: false, measure: false);

    // Tạo đủ 100000 khách trong Queue trước khi bắt đầu đo.
    var customers = new Queue<Customer>();
    for (int i = 1; i <= 100000; i++)
    {
        // Tạo khách i theo dữ liệu benchmark được yêu cầu.
        customers.Enqueue(new Customer(i, i - 1, 10));
    }

    // Đo mô phỏng và thống kê, không in từng khách trong lúc đo.
    SimulationResult result = Simulate(customers, 10, printCustomers: false, measure: true);

    // In phạm vi đo, thống kê và số đo thực tế sau khi hoàn tất.
    Console.WriteLine("Benchmark: 100000 khách, 10 quầy; không in từng khách.");
    Console.WriteLine("Phạm vi đo: tạo mảng thời điểm rảnh, xử lý hết Queue và tính thống kê.");
    Console.WriteLine("Kết quả mong đợi: Cho trung binh 0.00; Cho lau nhat 0; Hoan thanh luc 100009.");
    PrintStatistics(result);
    Console.WriteLine(
        $"Trạng thái: {(result.AverageWait == 0 && result.MaxWait == 0 && result.FinishTime == 100009 ? "PASS" : "FAIL")}");
    Console.WriteLine($"Thời gian: {result.Elapsed.TotalMilliseconds:F3} ms");
    Console.WriteLine($"Dưới 1000 ms: {(result.Elapsed.TotalMilliseconds < 1000.0 ? "CÓ" : "KHÔNG")}");
}

// Mô phỏng hàng khách theo FIFO và có thể tùy chọn in chi tiết hoặc đo hiệu năng.
static SimulationResult Simulate(
    Queue<Customer> customers,
    int k,
    bool printCustomers,
    bool measure)
{
    // Bắt đầu đo trước khi tạo mảng để tính cả phần khởi tạo quầy.
    Stopwatch? stopwatch = measure ? Stopwatch.StartNew() : null;

    // Lưu số khách trước khi Queue bị lấy hết để tính trung bình chính xác.
    int customerCount = customers.Count;

    // Mỗi ô lưu thời điểm một quầy rảnh; ban đầu tất cả đều bằng 0.
    long[] available = new long[k];

    // Dùng long vì tổng thời gian chờ có thể vượt giới hạn int.
    long totalWait = 0;

    // Ghi nhận thời gian chờ lớn nhất trong các khách đã xử lý.
    long maxWait = 0;

    // Ghi nhận thời điểm hoàn thành muộn nhất của tất cả quầy.
    long finishTime = 0;

    // Phục vụ cho đến khi không còn khách trong hàng.
    while (customers.Count > 0)
    {
        // Lấy khách đầu hàng để bảo đảm người đến trước được xử lý trước.
        Customer customer = customers.Dequeue();

        // Tạm chọn quầy đầu tiên làm quầy rảnh sớm nhất.
        int selected = 0;

        // So sánh với các quầy còn lại để tìm thời điểm rảnh nhỏ nhất.
        for (int j = 1; j < k; j++)
        {
            // Chỉ đổi khi rảnh sớm hơn; nếu bằng nhau giữ quầy số nhỏ hơn.
            if (available[j] < available[selected])
            {
                // Ghi nhận chỉ số quầy tốt hơn vừa tìm được.
                selected = j;
            }
        }

        // Khách chỉ bắt đầu khi đã đến và quầy đã rảnh.
        long start = Math.Max(customer.Arrival, available[selected]);

        // Thời gian chờ là khoảng từ lúc đến đến khi bắt đầu phục vụ.
        long wait = start - customer.Arrival;

        // Thời điểm xong bằng thời điểm bắt đầu cộng thời gian phục vụ.
        long end = start + customer.Service;

        // Quầy vừa chọn sẽ bận đến thời điểm khách này được phục vụ xong.
        available[selected] = end;

        // Cộng thời gian chờ để cuối cùng tính trung bình.
        totalWait += wait;

        // Cập nhật thời gian chờ lớn nhất.
        maxWait = Math.Max(maxWait, wait);

        // Cập nhật thời điểm hoàn thành của toàn bộ hệ thống.
        finishTime = Math.Max(finishTime, end);

        // Chỉ in chi tiết khách trong chế độ stdin thông thường.
        if (printCustomers)
        {
            // Đổi chỉ số quầy từ bắt đầu bằng 0 sang số quầy bắt đầu bằng 1.
            Console.WriteLine(
                $"KH {customer.Id}: den {customer.Arrival}, quay {selected + 1}, " +
                $"bat dau {start}, cho {wait}, xong {end}");
        }
    }

    // Chuyển sang double trước khi chia để giữ phần thập phân như chế độ hiện tại.
    double averageWait = (double)totalWait / customerCount;

    // Dừng đồng hồ sau khi đã tính xong đầy đủ các thống kê.
    stopwatch?.Stop();

    // Trả lại thống kê và thời gian đo cho bên gọi.
    return new SimulationResult(
        averageWait,
        maxWait,
        finishTime,
        stopwatch?.Elapsed ?? TimeSpan.Zero);
}

// In ba dòng thống kê theo định dạng hiện có.
static void PrintStatistics(SimulationResult result)
{
    // In trung bình với hai chữ số thập phân và dấu chấm.
    Console.WriteLine(
        "Cho trung binh: " +
        result.AverageWait.ToString("F2", CultureInfo.InvariantCulture));

    // In thời gian chờ lâu nhất trong tất cả khách.
    Console.WriteLine($"Cho lau nhat: {result.MaxWait}");

    // In thời điểm tất cả khách đã được phục vụ xong.
    Console.WriteLine($"Hoan thanh luc: {result.FinishTime}");
}

// Mỗi khách lưu số thứ tự, thời điểm đến và thời gian phục vụ.
public record Customer(int Id, long Arrival, long Service);

// Lưu các thống kê hoàn tất và thời gian đo của một lần mô phỏng.
public record SimulationResult(
    double AverageWait,
    long MaxWait,
    long FinishTime,
    TimeSpan Elapsed);
