#!/bin/bash

# Dừng khi dùng biến chưa khởi tạo và báo lỗi trong pipeline kiểm thử.
set -uo pipefail

# Dùng đường dẫn tuyệt đối để task chạy được từ workspace root.
project="/Users/minhnguyet/Documents/CTDLGT-Buoi03/Bai2/Bai2.csproj"

# Lưu toàn bộ báo cáo kiểm thử và số đo benchmark trong thư mục dự án.
results_log="/Users/minhnguyet/Documents/CTDLGT-Buoi03/Bai2/test-results.log"

# Tạo các file tạm trong Bai2; chỉ lưu stdout đầy đủ của ca lớn vào file riêng.
large_input=$(mktemp "/Users/minhnguyet/Documents/CTDLGT-Buoi03/Bai2/.case6-input.XXXXXX")
large_output="/Users/minhnguyet/Documents/CTDLGT-Buoi03/Bai2/case6-output.log"
large_error=$(mktemp "/Users/minhnguyet/Documents/CTDLGT-Buoi03/Bai2/.case6-error.XXXXXX")

# Xóa đúng các file tạm đã tạo khi script kết thúc.
trap 'rm -f "$large_input" "$large_error"' EXIT

# Bắt đầu log mới và gửi đồng thời toàn bộ báo cáo ra Terminal cùng file.
: > "$results_log"
exec > >(tee -a "$results_log") 2>&1

# Khai báo input cho năm ca nhỏ theo đúng thứ tự đề bài.
inputs=(
    $'3 1\n0 5\n1 3\n2 4'
    $'3 2\n0 5\n1 3\n2 4'
    $'3 2\n0 3\n0 3\n0 3'
    $'2 1\n0 2\n10 3'
    $'1 3\n4 6'
)

# Khai báo đủ từng dòng output mong đợi cho năm ca nhỏ.
expected=(
    $'KH 1: den 0, quay 1, bat dau 0, cho 0, xong 5\nKH 2: den 1, quay 1, bat dau 5, cho 4, xong 8\nKH 3: den 2, quay 1, bat dau 8, cho 6, xong 12\nCho trung binh: 3.33\nCho lau nhat: 6\nHoan thanh luc: 12'
    $'KH 1: den 0, quay 1, bat dau 0, cho 0, xong 5\nKH 2: den 1, quay 2, bat dau 1, cho 0, xong 4\nKH 3: den 2, quay 2, bat dau 4, cho 2, xong 8\nCho trung binh: 0.67\nCho lau nhat: 2\nHoan thanh luc: 8'
    $'KH 1: den 0, quay 1, bat dau 0, cho 0, xong 3\nKH 2: den 0, quay 2, bat dau 0, cho 0, xong 3\nKH 3: den 0, quay 1, bat dau 3, cho 3, xong 6\nCho trung binh: 1.00\nCho lau nhat: 3\nHoan thanh luc: 6'
    $'KH 1: den 0, quay 1, bat dau 0, cho 0, xong 2\nKH 2: den 10, quay 1, bat dau 10, cho 0, xong 13\nCho trung binh: 0.00\nCho lau nhat: 0\nHoan thanh luc: 13'
    $'KH 1: den 4, quay 1, bat dau 4, cho 0, xong 10\nCho trung binh: 0.00\nCho lau nhat: 0\nHoan thanh luc: 10'
)

# Build Debug đúng một lần trước các ca chạy chế độ stdin.
printf '%s\n' '=== Build Debug (một lần) ==='
if ! dotnet build "$project" --configuration Debug; then
    printf '%s\n' 'BUILD DEBUG FAIL; các ca stdin chưa được chạy.'
    exit 1
fi

# Khởi tạo bộ đếm kết quả cho cả sáu ca.
passed=0
failed=0

# Chạy và so sánh toàn bộ stdout của năm ca nhỏ.
for index in "${!inputs[@]}"; do
    case_number=$((index + 1))
    printf '\n=== Ca %d ===\n' "$case_number"
    printf 'Dữ liệu vào:\n%s\n' "${inputs[$index]}"
    printf 'Kết quả mong đợi:\n%s\n' "${expected[$index]}"

    # Truyền dữ liệu qua stdin và giữ nguyên lỗi thực tế của tiến trình.
    if actual=$(printf '%s\n' "${inputs[$index]}" | dotnet run --no-build --configuration Debug --project "$project" 2>&1); then
        run_status=0
    else
        run_status=$?
    fi

    # In stdout/stderr thực tế, không thay thế bằng kết quả mong đợi.
    printf 'Kết quả thực tế:\n%s\n' "$actual"

    # So sánh toàn bộ các dòng và tính cả lỗi thoát tiến trình là FAIL.
    if (( run_status == 0 )) && [[ "$actual" == "${expected[$index]}" ]]; then
        printf '%s\n' 'Trạng thái: PASS'
        passed=$((passed + 1))
    else
        printf 'Trạng thái: FAIL (exit code %d hoặc output không khớp)\n' "$run_status"
        failed=$((failed + 1))
    fi
done

# Ghi header dữ liệu ca 6 trước khi sinh đủ 100000 khách.
printf '100000 10\n' > "$large_input"

# Sinh input ca lớn ngoài tiến trình được đo; arrival=i-1 và service=10.
for ((i = 1; i <= 100000; i++)); do
    printf '%d 10\n' "$((i - 1))" >> "$large_input"
done

# Chạy chương trình Debug không build lại và chuyển toàn bộ stdout vào file log.
printf '\n=== Ca 6 — dữ liệu lớn ===\n'
printf '%s\n' 'Dữ liệu vào: tự sinh 100000 khách, 10 quầy; arrival=i-1, service=10.'
if dotnet run --no-build --configuration Debug --project "$project" < "$large_input" > "$large_output" 2> "$large_error"; then
    large_run_status=0
else
    large_run_status=$?
fi

# Ghi vị trí file chứa stdout đầy đủ của ca lớn vào báo cáo ngắn.
printf 'Toàn bộ stdout ca 6 được lưu tại: %s\n' "$large_output"

# Hiển thị stderr thật nếu tiến trình có ghi lỗi.
if [[ -s "$large_error" ]]; then
    printf '%s\n' 'Lỗi thực tế (stderr):'
    cat "$large_error"
fi

# Kiểm tra đủ 100000 dòng khách, từng dòng theo công thức, và cả ba thống kê.
if (( large_run_status == 0 )) && awk -v n=100000 '
    NR <= n {
        i = NR
        expected = sprintf("KH %d: den %d, quay %d, bat dau %d, cho 0, xong %d", i, i - 1, ((i - 1) % 10) + 1, i - 1, i + 9)
        if ($0 != expected) {
            bad++
            if (bad <= 5) {
                printf "Sai dòng khách %d: mong đợi [%s], thực tế [%s]\n", i, expected, $0
            }
        }
        next
    }
    NR == n + 1 { average = $0; next }
    NR == n + 2 { maximum = $0; next }
    NR == n + 3 { finish = $0; next }
    END {
        if (NR != n + 3) {
            printf "Sai tổng số dòng: mong đợi %d, thực tế %d\n", n + 3, NR
            bad++
        }
        if (average != "Cho trung binh: 0.00") {
            printf "Sai thống kê trung bình: [%s]\n", average
            bad++
        }
        if (maximum != "Cho lau nhat: 0") {
            printf "Sai thống kê chờ lâu nhất: [%s]\n", maximum
            bad++
        }
        if (finish != "Hoan thanh luc: 100009") {
            printf "Sai thống kê hoàn thành: [%s]\n", finish
            bad++
        }
        printf "Số dòng khách đã kiểm tra: %d/%d\n", (NR < n ? NR : n), n
        exit (bad > 0)
    }
' "$large_output"; then
    large_validation=0
else
    large_validation=1
fi

# In ba dòng khách đầu, ba dòng khách cuối và ba dòng thống kê thực tế của ca 6.
printf '%s\n' 'Vài dòng đầu thực tế:'
head -n 3 "$large_output"
printf '%s\n' 'Vài dòng cuối thực tế:'
sed -n '99998,100000p' "$large_output"
printf '%s\n' 'Ba dòng thống kê thực tế:'
sed -n '100001,100003p' "$large_output"

# Cập nhật tổng kết ca 6 theo trạng thái chạy và kiểm tra toàn bộ dữ liệu.
if (( large_run_status == 0 && large_validation == 0 )); then
    printf '%s\n' 'Trạng thái ca 6: PASS'
    passed=$((passed + 1))
else
    printf 'Trạng thái ca 6: FAIL (exit code %d, validation %d)\n' "$large_run_status" "$large_validation"
    failed=$((failed + 1))
fi

# Build Release đúng một lần để đo benchmark tối ưu hóa.
printf '\n%s\n' '=== Build Release (một lần) ==='
if ! dotnet build "$project" --configuration Release; then
    printf '%s\n' 'BUILD RELEASE FAIL; benchmark chưa được chạy.'
    printf 'Tổng ca: 6 | PASS: %d | FAIL: %d\n' "$passed" "$((failed + 1))"
    exit 1
fi

# Chạy benchmark Release không build lại; stdout benchmark đồng thời được log.
printf '\n%s\n' '=== Benchmark Release ==='
if dotnet run --no-build --configuration Release --project "$project" -- --benchmark; then
    benchmark_status=0
else
    benchmark_status=$?
    printf 'Benchmark lỗi thực tế (exit code %d).\n' "$benchmark_status"
fi

# In bảng tổng kết đủ sáu ca kiểm thử và thoát khác 0 nếu có ca thất bại.
printf '\n%s\n' '=== Bảng tổng kết 6 ca ==='
printf 'PASS: %d\n' "$passed"
printf 'FAIL: %d\n' "$failed"
printf 'Log báo cáo: %s\n' "$results_log"
printf 'Stdout đầy đủ ca 6: %s\n' "$large_output"

# Đánh dấu benchmark hỏng là lỗi thực thi thay vì giả lập số đo.
if (( benchmark_status != 0 )); then
    exit "$benchmark_status"
fi

# Trả về thất bại khi một hoặc nhiều ca kiểm thử không đạt.
if (( failed > 0 )); then
    exit 1
fi
