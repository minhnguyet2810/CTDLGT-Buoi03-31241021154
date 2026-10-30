# Kết quả thực nghiệm

## Môi trường

- **Hệ điều hành:** macOS
- **.NET SDK:** 10.0.401
- **Cấu hình chạy benchmark:** Release
- **CPU/RAM:** Không có thông tin được ghi nhận; không suy đoán cấu hình phần cứng.

## Tổng hợp kiểm thử

| Bài | Số ca/nhóm | Nội dung kiểm thử | Kết quả đã ghi nhận |
|---|---:|---|---|
| Bai1 | 10 ca | 8 ca chức năng: chuỗi ngoặc hợp lệ, ngoặc sai loại, ngoặc mở/đóng thừa, chuỗi rỗng và ký tự không phải ngoặc; 2 ca benchmark với chuỗi dài một triệu ký tự. | Đạt 10/10 theo kết quả đã ghi nhận. |
| Bai2 | 6 ca | 5 ca nhỏ: một quầy, hai quầy, khách đến cùng lúc, quầy rảnh trước lượt khách tiếp theo, một khách với ba quầy. Ca 6 mô phỏng 100.000 khách và kiểm tra từng dòng cùng thống kê. | Đạt 6/6 theo log; ca lớn đối chiếu 100.000 dòng khách và ba dòng thống kê. |
| Bai3 | 7 ca | Hàng rỗng; hàng vừa đầy; quay vòng; mở rộng khi front bằng 0; mở rộng khi front khác 0; một triệu lần thêm rồi lấy; thêm 100.000 phần tử rồi lấy hết. | Kết quả khớp 7/7 theo số liệu đã ghi nhận. |
| Bai4 phương án B | 4 nhóm, 12 phép kiểm tra | Push/GetMin; Pop/GetMin; lấy phần tử cuối; kiểm tra ngoại lệ `InvalidOperationException` của `GetMin` và `Pop` khi rỗng. | PASS 12/12 theo số liệu đã ghi nhận. |

### Ghi chú về dữ liệu kiểm thử

- Mã nguồn hiện tại cho thấy Bai1 và Bai2 có chế độ stdin cùng `--benchmark`; Bai3 và Bai4 tự chạy các ca kiểm thử trong chương trình.
- Workspace có log kết quả Bai2 tại `Bai2/test-results.log` và stdout đầy đủ của ca lớn tại `Bai2/case6-output.log`.
- Không tìm thấy log kết quả riêng cho Bai1, Bai3 hoặc Bai4 trong workspace. Các tổng số ca và kết quả của ba bài này trong bảng là số liệu đã được ghi nhận/cung cấp, không phải kết quả vừa chạy lại để tạo tài liệu.
- Không tạo hoặc gắn đường dẫn ảnh minh chứng trong tài liệu này.

## Số đo hiệu năng đã ghi nhận

| Bài/ca | Dữ liệu | Thời gian |
|---|---|---:|
| Bai1 ca 9 | 500.000 ngoặc mở và 500.000 ngoặc đóng | 3,797 ms |
| Bai1 ca 10 | 1.000.000 ngoặc mở chưa đóng | 5,254 ms |
| Bai2 ca 6 | 100.000 khách, 10 quầy | 5,016 ms |
| Bai3 ca 7 | Thêm 100.000 phần tử rồi lấy và đối chiếu toàn bộ | 1,855 ms |
| Bai4 phương án B | Không đo thời gian | — |

## Phạm vi đo

- **Bai1:** Đồng hồ đo lời gọi kiểm tra ngoặc, bao gồm tạo Stack bên trong hàm. Không tính sinh chuỗi, build, khởi động chương trình hoặc in kết quả.
- **Bai2:** Đồng hồ đo mô phỏng từ lúc bắt đầu tạo mảng thời điểm rảnh đến khi xử lý hết Queue và tính thống kê. Queue dữ liệu đã được tạo trước đó; không tính sinh/khởi tạo Queue đầu vào hoặc in kết quả.
- **Bai3 ca 7:** Đồng hồ bắt đầu trước khi tạo hàng đo chính thức và dừng sau khi thêm, lấy, đối chiếu 100.000 phần tử. Do đó thời gian có tính tạo hàng; không tính in kết quả.
- **Bai4 phương án B:** Không có số đo thời gian.

Các thời gian trên là số liệu đã ghi nhận trên máy sinh viên; không được đo lại khi tạo tài liệu này.
