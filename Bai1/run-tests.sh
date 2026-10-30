#!/bin/bash

set -uo pipefail

project="/Users/minhnguyet/Documents/CTDLGT-Buoi03/Bai1/Bai1.csproj"

printf '%s\n' '=== Build dự án Bai1 (một lần) ==='
if ! dotnet build "$project"; then
    printf '%s\n' 'BUILD FAIL: dừng kiểm thử vì build không thành công.'
    exit 1
fi

inputs=(
    '{(a+b)×[c−d]}'
    '{(a+b]×c)'
    '(a+b'
    'a+b)'
    ''
    '([)]'
    'a+b'
    '{[('
)

expected=(
    'HOP LE'
    'SAI tai vi tri 5'
    'SAI tai vi tri 0'
    'SAI tai vi tri 3'
    'HOP LE'
    'SAI tai vi tri 2'
    'HOP LE'
    'SAI tai vi tri 0'
)

passed=0
failed=0

printf '\n%s\n' '=== Chạy 8 ca kiểm thử (--no-build) ==='
for index in "${!inputs[@]}"; do
    case_number=$((index + 1))
    input="${inputs[$index]}"
    expected_output="${expected[$index]}"
    if [[ -z "$input" ]]; then
        display_input='(chuỗi rỗng)'
    else
        display_input="$input"
    fi

    printf '\n--- Ca %d ---\n' "$case_number"
    printf 'Dữ liệu vào: %s\n' "$display_input"
    printf 'Kết quả mong đợi: %s\n' "$expected_output"

    if actual_output=$(printf '%s\n' "$input" | dotnet run --no-build --project "$project" 2>&1); then
        printf 'Kết quả thực tế: %s\n' "$actual_output"
        if [[ "$actual_output" == "$expected_output" ]]; then
            printf '%s\n' 'Trạng thái: PASS'
            passed=$((passed + 1))
        else
            printf '%s\n' 'Trạng thái: FAIL'
            failed=$((failed + 1))
        fi
    else
        run_status=$?
        printf 'Lỗi chạy thực tế (exit code %d):\n%s\n' "$run_status" "$actual_output"
        printf '%s\n' 'Trạng thái: FAIL'
        failed=$((failed + 1))
    fi
done

printf '\n%s\n' '=== Bảng tổng kết ==='
printf 'Tổng số ca: %d\n' "${#inputs[@]}"
printf 'PASS: %d\n' "$passed"
printf 'FAIL: %d\n' "$failed"

if (( failed > 0 )); then
    exit 1
fi
