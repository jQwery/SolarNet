import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AUTH_API_URL, STORE_API_URL } from '../../models/app-injection-token';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { Token } from '../../models/token';
import {tap} from 'rxjs/internal/operators'
import { AppService } from 'src/app/app.service';

export const ACCES_TOKEN_KEY = 'solar_access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationServiceService {

  token: string = '';//переменная для токена

  constructor(
    private http: HttpClient, 
    @Inject(AUTH_API_URL) private apiUrl: string,
    @Inject(STORE_API_URL) private storeUrl: string,
    private JwtHelper: JwtHelperService,
    private router: Router,
    private service: AppService
    ) { }

  login(email: string, password: string): Observable<Token>{//функция оправляет на сервер введенные данные и получает токен, сохраняет его в хранилище
      return this.http.post<Token>(`${this.apiUrl}api/Auth/login`, {
        email, password
      }).pipe(
        tap(token=>{
          localStorage.setItem(ACCES_TOKEN_KEY, token.access_token);
        })
      )
  }

  isAuthenticated(): boolean{//Проверка авторизованности пользователя
    var token = localStorage.getItem(ACCES_TOKEN_KEY);
    return ((token!=null) && !this.JwtHelper.isTokenExpired(token));
  }

  logout(): void{//Выход из учетной записи
    localStorage.removeItem(ACCES_TOKEN_KEY);
    this.router.navigate(['']);
    this.service.setLoggedStatus("nd");
  }

  isLogged: boolean = false;

  private isLoggedInfo  = new BehaviorSubject(this.isLogged);

  getIsLoggedInfo(): Observable<boolean>{//возвращает изменения в переменной, которвя передана как аргумент обьекту выше
    return this.isLoggedInfo.asObservable();
  }

  getIsLogged(): boolean{//в обьекте так же есть геттер
    return this.isLoggedInfo.getValue();
  }
  
  setIsLoggedInfo(paramsList: boolean): void{//и сеттер
    this.isLoggedInfo.next(paramsList);
  }

  getCapcha(): Observable<string>{//Получение уникального номера, имитирующего проверку от робота
    return this.http.get<string>(`${this.storeUrl}api/User/capcha/get`);
  }

  registerUser(){//Запрос на регистрацию пользователя
    return this.http.post(`${this.storeUrl}api/User/register`, this.regFormValues);
  }

  regFormValues: any = {//форма регистрации
    name: '',
    email: '',
    password: '',
    phone: '',
    code: ''
  }

  clearRegFormValues(): void{//очистка формы регистрации
    this.regFormValues.name = '';
    this.regFormValues.mail = '';
    this.regFormValues.phoneNumber = '';
    this.regFormValues.password = '';
    this.regFormValues.passwordAgain = '';
    this.regFormValues.notRobotCode = '';
  }

  signInFormValues: any = {//форма входа
    mail: '',
    password: ''
  }

  clearSignInFormValues(): void{//очистка формы входа
    this.signInFormValues.mail = '';
    this.signInFormValues.password = '';
  }
}
