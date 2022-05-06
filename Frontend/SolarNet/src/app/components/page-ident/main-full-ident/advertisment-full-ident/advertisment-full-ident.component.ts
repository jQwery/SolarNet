import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { MainIdentServiceService } from '../../main-ident/main-ident-service.service';
import { MainFullIdentService } from '../main-full-ident.service';

@Component({
  selector: 'app-advertisment-full-ident',
  templateUrl: './advertisment-full-ident.component.html',
  styleUrls: ['./advertisment-full-ident.component.css']
})
export class AdvertismentFullIdentComponent implements OnInit {
  
  i = 0;

  // authorId = 1;// переменная для хранения информации об авторе, должна быть взята из самого главного сервиса

  value: any;//переменная для хранения данных обьявления

  date: string = '';

  commentValues: any = {//Обьект комментария
    text: '',
  }

  startBtnView: boolean = true;

  constructor(private appServ: AppService, private mFServ: MainFullIdentService, private router: Router, private mServ: MainIdentServiceService) {
  }

  ngOnInit(): void {
    // console.log(this.value);
    this.returnNormalizedValue();
  }

  months: any[] = ["января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря"];

  closeConfirmBtns(): void{//Отображение кнопок для подтверждения удаления обьявления
    this.startBtnView = true;
  }

  openConfirmBtns(): void{
    this.startBtnView = false;
  }

  returnNormalizedValue(): void{//Возврат даты в нормальном виде
    let day = this.value.date.slice(0, this.value.date.indexOf('.'));
    let cPos = this.value.date.indexOf('.');
    let month = this.value.date.slice(cPos+1, this.value.date.indexOf('.', cPos+1));
    cPos = this.value.date.indexOf('.', cPos+1);
    let year = this.value.date.slice(cPos+1);
    this.date = day + ' ' + this.months[month-1] + ' ' + year;
  }

  saveCommentValues(): void{//Создание обьекта обьявления для его создания
    let newComment = document.querySelector('.new-comment') as HTMLTextAreaElement;
    if(this.commentValues.text.length < 3){
      newComment.style.borderColor = 'red';
    }else{
      newComment.style.borderColor = '#FFF622';
      this.commentValues.advertismentId = this.value.id;//добавление информации об обьявлении, к которому относится коммент
      newComment.classList.add('none');
      console.log(this.commentValues);
    }
    
  }

  clearCommentValues(): void{//очистка данных комментария
    this.commentValues.text = '';//так как при создании комментария все остальные поля и так перезапишутся, то их можно не трогать
    //а это поле зависит от шаблона
    let newComment = document.querySelector('.new-comment') as HTMLTextAreaElement;
    newComment.classList.add('none');
    console.log(this.commentValues);
  }

  toggleCommentBar(): void{// просто свернет текстовое поле, без очистки обьекта комментария
    let newComment = document.querySelector('.new-comment') as HTMLTextAreaElement;
    newComment.classList.toggle('none');
  }

  getLoggedStatus(): string{//Проверка роли пользователя
    return this.appServ.getLoggedStatus();
  }

  showPhoneNumber(ev: Event): void{//При клике на номер телефона автора обьявления, он будет показываться
    let tg = ev.target as HTMLTextAreaElement;
    tg.innerHTML = this.value.userPhone;
  }

  slider(ev:Event): void{// перелистывание фото по стрелочкам
    let tg = ev.target as HTMLTextAreaElement;
    let img = document.querySelector('.adv-main-img') as HTMLImageElement;
    if(tg.classList.contains('left')){
      this.i--;
      if(this.i < 0){
        this.i = (this.value.images.length)-1;
      }      
      img.src =  this.value.images[this.i];
    }else{
      this.i++;
      
      if(this.i > (this.value.images.length)-1){
        this.i = 0;
      }
      img.src =  this.value.images[this.i];
    }
  }

  submit(commentValues: any) {//Запрос на добавление комментария
    this.mFServ.addComment(commentValues).subscribe((data) => {
        console.log(data);
        this.mFServ.setadvChangedInfo("new");
      },
      (error) => console.log(error)
    )
  }

  addOtherUserAdvList(): void{//смена статуса пользователя
    this.mServ.otherUserId = this.value.userId;
    this.mServ.otherUserName = this.value.userName;
    this.mServ.setAdvsType('otherUser');
  }

  deleteAdv(advId: any): void{//Удаление обьявления
    this.mServ.deleteAdv(advId).subscribe(data => {
      console.log(data);
      this.router.navigate(['']);
    },(error)=>{console.log(error)})
  }
}
