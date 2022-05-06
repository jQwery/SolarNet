import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppService {

  private isLogged = "nd";//отображение текущей роли пользователя

  getLoggedStatus(): string{
    return this.isLogged;
  }

  setLoggedStatus(newStatus:string): void{//Статус пользователя
    this.isLogged = newStatus;
  }

  constructor() { }
}
