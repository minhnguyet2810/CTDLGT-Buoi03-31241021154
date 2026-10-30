# Bài thực hành Cấu trúc dữ liệu và giải thuật — Buổi 03

## Thông tin sinh viên

- **Họ tên:** Nguyễn Minh Nguyệt
- **MSSV:** 31241021154
- **Lớp học phần:** 26C1INF50900706
- **Môn học:** Cấu trúc dữ liệu và giải thuật
- **Tên repository dự kiến:** `CTDLGT-Buoi03-31241021154`

## Yêu cầu môi trường

- .NET SDK 10
- Phiên bản SDK đã dùng: **10.0.401**

## Cấu trúc thư mục

| Thư mục | Nội dung |
|---|---|
| `Bai1/` | Kiểm tra tính hợp lệ của các cặp dấu ngoặc |
| `Bai2/` | Mô phỏng khách được phục vụ tại nhiều quầy |
| `Bai3/` | Hàng đợi FIFO bằng mảng vòng có khả năng tự mở rộng |
| `Bai4/` | MinStack, phương án B dùng danh sách liên kết |

## Cách chạy

Chạy các lệnh sau từ thư mục gốc của workspace:

```sh
dotnet run --configuration Release --project Bai1
dotnet run --configuration Release --project Bai2
dotnet run --configuration Release --project Bai3
dotnet run --configuration Release --project Bai4
```

Bai1 và Bai2 đọc dữ liệu từ **stdin**. Có thể nhập dữ liệu trực tiếp sau khi chạy lệnh hoặc chuyển nội dung từ file/pipe vào chương trình.

Bai3 và Bai4 tự chạy các nhóm kiểm thử được viết trong `Program.cs`, không yêu cầu nhập dữ liệu.

Bai1 và Bai2 có chế độ benchmark tự sinh dữ liệu:

```sh
dotnet run --configuration Release --project Bai1 -- --benchmark
dotnet run --configuration Release --project Bai2 -- --benchmark
```

Benchmark Bai1 kiểm tra hai chuỗi ngoặc dài một triệu ký tự. Benchmark Bai2 mô phỏng 100.000 khách với 10 quầy. Phạm vi đo cụ thể được ghi trong [KetQuaThucNghiem.md](./KetQuaThucNghiem.md).

## Tóm tắt giải pháp và độ phức tạp

| Bài | Giải pháp | Thời gian | Bộ nhớ phụ |
|---|---|---:|---:|
| Bai1 | Dùng Stack lưu ngoặc mở cùng vị trí; ghép ngoặc đóng và tìm vị trí ngoặc mở chưa đóng sớm nhất khi cần. | Xấu nhất Θ(n) | O(n) |
| Bai2 | Dùng Queue để giữ thứ tự khách; với mỗi khách quét k quầy để chọn quầy rảnh sớm nhất. | Θ(n × k) | Θ(n + k) |
| Bai3 | Hàng đợi FIFO dùng mảng vòng, tăng gấp đôi dung lượng khi đầy. | `Enqueue`: Θ(1) khấu hao, Θ(n) khi mở rộng; `Dequeue`, `Peek`, `Count`, `Capacity`: Θ(1) | Θ(Capacity) |
| Bai4 phương án B | MinStack bằng các nút liên kết; mỗi nút lưu giá trị và giá trị nhỏ nhất tại độ sâu đó. | `Push`, `Pop`, `GetMin`, `Count`: O(1) | O(n) |

## Kiểm thử và số đo

- **Bai1:** 10 ca — 8 ca chức năng và 2 ca benchmark; đã ghi nhận đạt 10/10.
- **Bai2:** 6 ca, gồm 5 ca nhỏ và 1 ca 100.000 khách; đã ghi nhận đạt 6/6.
- **Bai3:** 7 ca tự kiểm thử, gồm các tình huống hàng rỗng/đầy, quay vòng, mở rộng, một triệu lần thêm-lấy và ca 100.000 phần tử; đã ghi nhận kết quả khớp 7/7.
- **Bai4 phương án B:** 4 nhóm, tổng cộng 12 phép kiểm tra; đã ghi nhận PASS 12/12.

| Bài/ca | Thời gian đã ghi nhận |
|---|---:|
| Bai1 ca 9 | 3,797 ms |
| Bai1 ca 10 | 5,254 ms |
| Bai2 ca 6 | 5,016 ms |
| Bai3 ca 7 | 1,855 ms |
| Bai4 phương án B | Không đo thời gian |

Chi tiết nhóm kiểm thử, phạm vi đo và ghi chú về bằng chứng được trình bày trong [KetQuaThucNghiem.md](./KetQuaThucNghiem.md).

## Khai báo sử dụng AI

ChatGPT hỗ trợ giải thích, gợi ý và chỉnh sửa mã. GitHub Copilot Agent hỗ trợ chạy kiểm thử tự động và chuẩn bị tài liệu. Các kiểm thử được chạy trên máy cá nhân; kết quả được đối chiếu và ảnh minh chứng được lưu để phục vụ báo cáo. Khai báo này không khẳng định sinh viên đã hiểu hoàn toàn hoặc tự viết toàn bộ mã.
