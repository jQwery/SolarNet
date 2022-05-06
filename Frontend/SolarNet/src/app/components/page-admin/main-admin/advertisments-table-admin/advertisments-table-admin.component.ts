import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { MainIdentServiceService } from 'src/app/components/page-ident/main-ident/main-ident-service.service';
import { AdvertismentAdminComponent } from './advertisment-admin/advertisment-admin.component';

@Component({
  selector: 'app-advertisments-table-admin',
  templateUrl: './advertisments-table-admin.component.html',
  styleUrls: ['./advertisments-table-admin.component.css']
})
export class AdvertismentsTableAdminComponent implements OnInit {

  @ViewChild('books', {read: ViewContainerRef}) book:any;
  
  interval: any;
  advId:number = 1;

  // sortForm: any = {
  //   byDescendingDate: '',
  //   byDescendingCost: ''
  // }

  constructor(private componentFactoryResolver: ComponentFactoryResolver, private mServ: MainIdentServiceService, private service: AppService) { 
    // this.mServ.getParamsInfo().subscribe(() =>{this.getAds()})
  }

  ngOnInit(): void {
    this.getAdminStatus();
  }
  
  getAdminStatus(): void{
    console.log(this.service.getLoggedStatus());
}


  // addBook(): void{//метод, добавляющий весь список обьявлений на стартовую страницу
  //   this.book.clear();//перед каждой новой отрисовкой всех обьялвений, представление стирается, чтобы они не наслаивались друг на друга
  //   this.getAdvertisments();
  // }

  // getAdvertisments(): void{
  //   this.mServ.getAdvertismentsFromServer().subscribe(data => {
  //     for( let i = 0; i < data.length; i++){
  //         let bookItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentAdminComponent)// берется шаблон компонента
  //         let bookItemComponentRef = this.book.createComponent(bookItemComponent);// по шаблону создается элемент
  //         console.log(data);
  //         (<AdvertismentAdminComponent>(bookItemComponentRef.instance)).value = data[i]//данные берутся из сервиса
  //     }
  //   })
  // }

    //метод, задающий тайм аут, интервал нулевой, но он позволяет перешагнуть на следующий макроэтап отображения контента
    //то есть к тому моменту когда все прогрузится, если его не использовать, то переменная в ViewChild не будет обработана и вернет 
    //undefined
// /
  // changeParamsForm(): void{
  //   this.mServ.setAllCategoryParamsSort(this.sortForm);
  // }

}
