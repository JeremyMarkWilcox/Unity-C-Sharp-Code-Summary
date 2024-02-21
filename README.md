# Unity/ C# Code Summary
 A 2D game created with the Unity Engine, written in C#


## Game Screens




## Game Movement




## Enemy Movement/ Spawning




## Score




## Units Customized 



## Array

### chunk

将数组划分成特定大小的小数组。

```java
public static int[][] chunk(int[] numbers, int size) {
    return IntStream.iterate(0, i -> i + size)
        .limit((long) Math.ceil((double) numbers.length / size))
        .mapToObj(cur -> Arrays.copyOfRange(numbers, cur, cur + size > numbers.length ? numbers.length : cur + size))
        .toArray(int[][]::new);
}
