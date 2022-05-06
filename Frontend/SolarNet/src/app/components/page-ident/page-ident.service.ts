import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { ACCES_TOKEN_KEY } from '../authorization/authorization-module/authorization-service.service';
import { STORE_API_URL } from '../models/app-injection-token';

@Injectable({
  providedIn: 'root'
})
export class PageIdentService {

  tokenDecode: any[] = [];

  userValues: any = {//обьект будет браться из основного сервиса, сейчас он заполнен для дальнейшей разработки и тестирования
    name: '',
    mail: '',
    phoneNumber: '',
    password: '',
    passwordAgain: '',
    passwordAgainSub: '',
    avatarLink: 'https://cs.pikabu.ru/post_img/big/2013/08/24/1/1377296637_1500370441.png'
  }

  private userValuesInfo = new BehaviorSubject(this.userValues);

  getUserValuesInfo(): Observable<any>{//возвращает изменения в переменной, которвя передана как аргумент обьекту выше
    return this.userValuesInfo.asObservable();
  }

  getUserValues(): any{//в обьекте так же есть геттер
    return this.userValuesInfo.getValue();
  }
  
  setUserValuesInfo(paramsList: any): void{//и сеттер
    this.userValuesInfo.next(paramsList);
  }

  clearUserValues(): void{//Очистка пользовательских данных
    this.userValues.name = '';
    this.userValues.mail = '';
    this.userValues.phoneNumber = '';
    this.userValues.avatarLink = '';
    this.userValues.password = '';
    this.userValues.passwordAgain = '';
    this.setUserValuesInfo(this.userValues);
  }

  addUserValues(): void{//Добавление пользовательских данных их токена
    this.userValues.name = this.tokenDecode[2];
    this.userValues.mail = this.tokenDecode[0];
    this.userValues.phoneNumber = this.tokenDecode[3];
    this.userValues.avatarLink = this.tokenDecode[4];
    this.setUserValuesInfo(this.userValues);
  }

  decodeToken(): void{//Расшифровка токена
    if(localStorage.getItem(ACCES_TOKEN_KEY) == null){
      this.tokenDecode = [];
    }else{
      this.tokenDecode = Object.values(this.jwtHelper.decodeToken(localStorage.getItem(ACCES_TOKEN_KEY)!));//значения обьекта расшифрованного токена
    // console.log(this.tokenDecode);
    this.addUserValues();
    }
  }

  constructor(private jwtHelper: JwtHelperService, 
              private http: HttpClient,
              @Inject(STORE_API_URL) private storeUrl: string) {
                // this.decodeToken(); 
  }

  changeUserValues(userValues: any): Observable<any>{//Запрос на изменение данных
    const body = {name: userValues.name, currentPassword: userValues.password, newPassword: userValues.passwordAgain, newEmail: userValues.mail, phone: userValues.phoneNumber}
    return this.http.post(`${this.storeUrl}api/User/change-data`, body)
  }

  changeUserAvatar(userValues: any): Observable<any>{//Смена автара
    return this.http.post(`${this.storeUrl}api/User/change-avatar`, {avatarLink: userValues.avatarLink})
  }

  uploadFile(formData: FormData): Observable<string> {//Загрузка файла
    return this.http.post(`${this.storeUrl}api/Image/upload`, formData, { responseType: 'text' });
  }
}




