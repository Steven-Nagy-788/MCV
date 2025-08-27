//1
let a = 10;
let b = 20;
console.log("a = " + a+", b = " + b);
[a,b] = [b,a];
console.log("a = " + a+", b = " + b);

//2
let num =[4,9,2,7,5];
let max = num[0];
console.log(num);
for(let n of num){
    if(n>max)
        max = n;
}
console.log("max = " + max);

//3
let str = "JavaScript is awesome";
let count = 0;
for(let s of str){
    if(s.toLowerCase() === 'a'||s.toLowerCase() === 'e'||s.toLowerCase() === 'i'||s.toLowerCase() === 'o'||s.toLowerCase() === 'u')
        count++;
}
console.log(str);
console.log("vowel count = " + count);

//4
function isPrime(pNum){
    if(pNum <= 1) return false;
    for(let i=2; i<pNum; i++){
        if(pNum % i === 0)
            return false;
    }
    return true;
}
console.log("isPrime(17) = " + isPrime(17));

//5
function reverseString(rStr){
    let reversed = "";
    for(let i = rStr.length-1 ; i>=0 ; i--){
        reversed += rStr[i];
    }
    return reversed;
}
console.log("reverseString('Hello World') = " + reverseString("Hello World"));

//6
let sumNum = [1, 2, 3, 4, 5, 6];
let sum = 0;
for(let n of sumNum){
    if(n % 2 === 0)
        sum += n;
}
console.log(sumNum);
console.log("sum of even numbers = " + sum);

//7
let arr = [1, 2, 3, 2, 4, 1, 5];
let uniqueArr =[];
for(let num in arr){
    if(!uniqueArr.includes(arr[num]))
        uniqueArr.push(arr[num]);
}
console.log("Og array ="+arr);
console.log("unique array ="+uniqueArr);

//9
function factorial(n){
    if(n === 0 || n === 1)
        return 1;
    for(let i=n-1; i>=1; i--){
        n *= i;
    }
    return n;
}
console.log("factorial(5) = " + factorial(5));

//10
let car = { brand: "Toyota", model: "Corolla", year: 2020, color: "blue" };
for(let info in car){
    console.log(info + ": " + car[info]);
}