import { Component, OnInit } from '@angular/core';
import { AuthorizationServiceService } from '../authorization-module/authorization-service.service';
import { FormsModule } from '@angular/forms';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  capcha: string = '';

  constructor(protected aServ: AuthorizationServiceService, public service: AppService) {
   this.aServ.getCapcha().subscribe(capcha =>{//Получение уникального номера при создании окна регистрации
     this.capcha = capcha
   })
  }

  regAccess: boolean = false;//Для отображения информации об успешной регистрации и дальнейших действиях пользователя в интерфейсе

  getRegValues(): any{//Получение данных регистрации
    return this.aServ.regFormValues;   
  }

  printForm(): void{
    console.log(this.aServ.regFormValues);
  }

  ngOnInit(): void {}

  ngOnDestroy(): void{
    this.aServ.clearRegFormValues();//функция очистки обьекта, где хранятся данные формы в сервисе
  }

  registerNewUser(){//Запрос на регистрацию пользователя
    this.aServ.registerUser().subscribe(() => {;
    this.regAccess = true;//Регистрация прошла успешно
    }, error =>{
      console.log(error);
      alert("Неизвестная ошибка, попробуйте ещё раз");//Вывод ошибки пользователю
    })
  }

}