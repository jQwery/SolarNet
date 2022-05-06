import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MainFullIdentService } from '../../../main-full-ident/main-full-ident.service';
import { MainIdentServiceService } from '../../main-ident-service.service';

@Component({
  selector: 'app-advertisment-ident',
  templateUrl: './advertisment-ident.component.html',
  styleUrls: ['./advertisment-ident.component.css']
})
export class AdvertismentIdentComponent implements OnInit {
  value: any;//переменная для хранения данных

  categoryId: any;//переменная для хранения информации о категории

  interval: any;

  mainImgLink: string = "";//Ссылка на главное фото

  date: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private mServ: MainIdentServiceService) { 
    
  }

  months: any[] = ["января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря"];

  returnNormalizedValue(): void{//Возврат нормальной даты
    let day = this.value.date.slice(0, this.value.date.indexOf('.'));
    let cPos = this.value.date.indexOf('.');
    let month = this.value.date.slice(cPos+1, this.value.date.indexOf('.', cPos+1));
    cPos = this.value.date.indexOf('.', cPos+1);
    let year = this.value.date.slice(cPos+1);
    this.date = day + ' ' + this.months[month-1] + ' ' + year;
  }

  ngOnInit(): void {
    this.categoryId = this.value?.categoryId;
    this.createImgList();//Динамическое создание картинок к обьявлению
    // console.log(this.value.status);
    this.returnNormalizedValue();
  }

  deleteAdv(advId: any): void{//Удаление обьявления
    this.mServ.deleteAdv(advId).subscribe(data => {
      console.log(data);
      // this.router.navigate(['']);
      this.mServ.setAdvsType('user');//Перерисовка списка обьявлений пользователя
    },(error)=>{console.log(error)})
  }

  createImgList(): void{//Создание из картинок для обьявления массива ссылок 
    if((this.value.images.length >= 1) && (this.value.images[0] != "string")){//Добавление ссылки к основному фото обьявления
      this.mainImgLink = this.value.images[0];
    }else{//еслм картинок нет, то добавляется картина с информацией о том, что фото нет
      this.mainImgLink = "https://www.teplogid.ru/uploads/prod/d0d9d53fd403dabb7d5b12009c9034ea.jpeg";
     }
  }

  addRoundBorders(): boolean{
    return this.value.images.length == 0 || this.value.images.length == 1;
  }

  manageAdv(): string{//Для администрировния обьявлений пользователя,
    if(this.value.canChange){//В зависимости от статуса, будет изменен внешний вид
      switch(this.value.status){
        case 'DeletedByAdmin':{
          return 'DeletedByAdmin'//Размытый фон и надпись
        }
        case 'Deleted':{
          return 'Deleted'//Размытый фон и надпись
        }
        case 'Created':{
          return 'Created'//Серое, еще непроверенное обьявление
        }
        case 'Rejected':{
          return 'Rejected'//Красное, отлоненное обьявление
        }
        case 'Active':{
          return 'Active'//Зеленое, одобренное обьявление
        }
      }return '';
    }else{
      return '';
    }
  }

  makeMainImgBig(ev: Event): void{//При клике на картинку обьявления, она увеличивается, и можно просмотреть все картинки не переходя в само обьявление
    let tg =  ev.target as HTMLTextAreaElement;
    if(!(tg.classList.contains('left') || tg.classList.contains('right'))){
      if(tg.classList.contains('main-img')){
        tg.classList.toggle('main-img-big');
      }else{
        tg = tg.parentElement as HTMLTextAreaElement;
        tg?.classList.toggle('main-img-big');
      }
      if((tg.parentElement?.children[1] != null)){
        tg.parentElement?.children[1].classList.toggle('none');
      }
      tg.children[0].classList.toggle('none');
      tg.children[2].classList.toggle('none');
    }
  }

  i = 0;

  slider(ev:Event){//Слайдер для переключения картинок в превью обьявления
    let tg = ev.target as HTMLTextAreaElement;
    if(this.value.images.length != 0){
      if(tg.classList.contains('left')){
        this.i--;
        if(this.i < 0){
          this.i = (this.value.images.length)-1;
        }      
        this.mainImgLink =  this.value.images[this.i];
        }else{
          this.i++;
          if(this.i > (this.value.images.length)-1){
            this.i = 0;
          }
        this.mainImgLink =  this.value.images[this.i];
      }
    }
  }

  addOtherUserAdvList(): void{//смена статуса пользователя
    this.mServ.otherUserId = this.value.userId;
    this.mServ.otherUserName = this.value.userName;
    this.mServ.setAdvsType('otherUser');
    this.router.navigate(['']);
  }

  goToAdv(ev:Event): void{//При клике на ссылку обьявления, осуществляется переход в полную версию обьявления
    let tg = ev.target as HTMLTextAreaElement;
    if(!tg.classList.contains('user-advs-btn')){
      this.router.navigate(['advertisment', this.value.id]);
    }
  }

  showConfirmBar(): void{//Окно для подтверждения действий
    let adv = document.querySelector('.a'+ this.value.id);
    adv?.querySelector('.confirm-approve')?.classList.toggle('none');
  }

}
