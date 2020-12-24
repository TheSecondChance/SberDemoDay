import time

def input_number_list(rows):
    lst = []
    for _ in range(rows):
        lst.append(list(map(int, input().split())))
    return lst

rows_number = int(input())
numbers_list = input_number_list(rows_number)

requests_number = int(input())
requests_list = input_number_list(requests_number)

start_time = time.time()
for request in requests_list:
    row, column = request
    if column > numbers_list[row - 1][0] or column < 1:
        print("ОШИБКА!")
    else:
        print(numbers_list[row - 1][column])

print(f'Число строк: {rows_number}')
print(f'Число запросов: {requests_number}')
print(f'Время выполнения: {(time.time() - start_time) * 1000} мс.')