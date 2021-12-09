def check(n):
    x = map(n.count, n)
    return list(n) == sorted(n) and 2 in map(n.count, n)

print(sum(check(str(n)) for n in range(357253, 892942)))

for n in range(357253, 892942):
    if (check(str(n))):
        print(n)