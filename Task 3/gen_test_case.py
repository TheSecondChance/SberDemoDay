import random

def vector_to_string(vect):
    return ' '.join(map(str, vect)) + '\n'

lines = []

rows_number = random.randint(500,1000)
requests_number = random.randint(1000,1500)

lines.append([rows_number])
for _ in range(rows_number):
    row = []
    columns_number = random.randint(0, 5000)
    row.append(columns_number)
    for _ in range(columns_number):
        row.append(random.randint(0, 10000))
    lines.append(row)

lines.append([requests_number])
for _ in range(requests_number):
    x, y = [random.randint(1,rows_number), random.randint(-5, 30)]
    lines.append([x,y])

lines_str = list(map(vector_to_string, lines))

with open("random_case1.txt", "w") as f:
    f.writelines(lines_str)