import os
count_file_cs = 0
def count_cs_lines(path):
    global count_file_cs
    total_lines = 0

    for root, dirs, files in os.walk(path):
        for file in files:
            if file.endswith(".cs"):
                count_file_cs += 1
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
                        lines = f.readlines()
                        line_count = len(lines)
                        total_lines += line_count
                        print(f"Đã đếm {line_count} dòng trong file: {file_path}")
                except Exception as e:
                    print(f"Lỗi khi đọc file {file_path}: {e}")
    
    return total_lines

if __name__ == "__main__":
    current_folder = os.getcwd()
    total = count_cs_lines(current_folder)
    print(f"\n🧮 Tổng số dòng trong tất cả file .cs: {total}")
    print(f"🧮 Tổng số file .cs: {count_file_cs}")
