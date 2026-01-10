import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})
export class RandomIdGenerator{
    public static counter = 0;
    public getRandomNumber(): string{
        RandomIdGenerator.counter++;
        return RandomIdGenerator.counter.toString();
    }
}