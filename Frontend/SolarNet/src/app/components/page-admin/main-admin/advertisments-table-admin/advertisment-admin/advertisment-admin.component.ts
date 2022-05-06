import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MainIdentServiceService } from 'src/app/components/page-ident/main-ident/main-ident-service.service';

@Component({
  selector: 'tr[app-advertisment-admin].table-body-line',
  templateUrl: './advertisment-admin.component.html',
  styleUrls: ['./advertisment-admin.component.css']
})
export class AdvertismentAdminComponent implements OnInit {

  value: any;//переменная для хранения данных

  categoryId: any;//переменная для хранения информации о категории

  advListType: any;

  date: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private mServ: MainIdentServiceService) { 
  }

  months: any[] = ["января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря"];

  categoriesNames: string[] = ['Транспорт', 'Вещи', 'Недвижимость', 'Электроника', 'Автомобили', 'Мотоциклы', 'Спецтехника', 'Водный транспорт', 'Одежда, обувь', 'Аксессуары', 'Для детей', 'Прочее',  'Квартиры', 'Дома', 'Комнаты', 'Гаражи',  'Телефоны', 'Ноутбуки', 'Комплектующие', 'Аксессуары'];


  returnNormalizedValue(): void{//Возвращает преобразованную в нормальный вид дату
    let day = this.value.date.slice(0, this.value.date.indexOf('.'));
    let cPos = this.value.date.indexOf('.');
    let month = this.value.date.slice(cPos+1, this.value.date.indexOf('.', cPos+1));
    cPos = this.value.date.indexOf('.', cPos+1);
    let year = this.value.date.slice(cPos+1);
    this.date = day + ' ' + this.months[month-1] + ' ' + year;
  }

  getAdvListType(): boolean{//Для отображения отдельного вида обьявлений для админа
    this.advListType = this.mServ.getParams().advListType;//Добавляется возможность одобрения/отклонения обьявления
    console.log(this.advListType);
    return this.advListType == 'notApproved';
  }

  approveAdv(): void{//Запрос на одобрение обьявления
    this.mServ.approveAdv(this.value.id).subscribe(data => {
      console.log(data);
      this.router.navigate(['']);
      this.mServ.setAdvsType('notApproved');//Далее сразу перезагрузка списка запросов, с учетом изменений
    }, error => {
      console.log(error);
    })
  }

  rejectAdv(): void{//Отклонение обьявления
    this.mServ.rejectAdv(this.value.id).subscribe(data => {
      console.log(data);
      this.router.navigate(['']);
      this.mServ.setAdvsType('notApproved');
    }, error => {
      console.log(error);
    })
  }

  ngOnInit(): void {
    this.categoryId = this.value?.categoryId;
    this.returnNormalizedValue();
  }

  showConfirmBar(ev: Event): void{//Сраху отклонить или одобрить не получится
    let adv = document.querySelector('.b'+ this.value.id);//Вывод модального окна для подтверждения действий
    let tg = ev.target as HTMLTextAreaElement;
    if(tg.classList.contains('a')){
      adv?.querySelector('.confirm-approve')?.classList.toggle('none');
    }else if(tg.classList.contains('n-a')){
      adv?.querySelector('.confirm-not-approve')?.classList.toggle('none');
    }
  }
}
