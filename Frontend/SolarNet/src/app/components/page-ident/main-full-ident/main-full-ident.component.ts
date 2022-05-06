import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MainIdentServiceService } from '../main-ident/main-ident-service.service';
import { AdvertismentFullIdentComponent } from './advertisment-full-ident/advertisment-full-ident.component';
import { CommentsIdentComponent } from './comments-ident/comments-ident.component';
import { MainFullIdentService } from './main-full-ident.service';

@Component({
  selector: 'app-main-full-ident',
  templateUrl: './main-full-ident.component.html',
  styleUrls: ['./main-full-ident.component.css']
})
export class MainFullIdentComponent implements OnInit {
  @ViewChild('fullAdv', {read: ViewContainerRef}) fullAdv:any;
  @ViewChild('advComments', {read: ViewContainerRef}) advComments:any;

  interval: any;
  advId: number = 0;

  constructor(
    private route: ActivatedRoute, private router: Router, 
    private componentFactoryResolver: ComponentFactoryResolver,
    private mFServ: MainFullIdentService,
    private mIServ: MainIdentServiceService) {
    this.route.params.subscribe(params => console.log(params));//подписка на изменение параметров роутинга
  }

  ngOnInit(): void {
    this.advId = this.route.snapshot.params.id;//извлечение параметров пути для текущего компонента, нас интересует только advId
    console.log(this.advId);                                              //так как он будет выступать индексом для извлечения нужных данных из массива данных 
    this.getAds();//добавление блока обьявления и комментариев
  }

  addAdv(): void{//функция добавления блока обьявления
    this.mFServ.getAdvValue(this.advId).subscribe(data => {
      console.log(data);
      let fullAdvItemComponent = this.componentFactoryResolver.resolveComponentFactory(AdvertismentFullIdentComponent)// берется шаблон компонента
      let fullAdvItemComponentRef = this.fullAdv.createComponent(fullAdvItemComponent);// по шаблону создается элемент
      (<AdvertismentFullIdentComponent>(fullAdvItemComponentRef.instance)).value = data;
    })
      
  }

  addComments(): void{//функция добавления блока комментариев
    let advCommentsItemComponent = this.componentFactoryResolver.resolveComponentFactory(CommentsIdentComponent)// берется шаблон компонента
    let advCommentsItemComponentRef = this.advComments.createComponent(advCommentsItemComponent);// по шаблону создается элемент
    (<CommentsIdentComponent>(advCommentsItemComponentRef.instance)).value = {
      id: this.advId
    };
  }

  getAds() {//тайм аут для правилной прогрузки динамических компонентов и добавлении их в шаблон без ошибок
      this.interval = setTimeout(() => {
        this.addAdv();
        this.addComments();
      }, 0);
  }
}
