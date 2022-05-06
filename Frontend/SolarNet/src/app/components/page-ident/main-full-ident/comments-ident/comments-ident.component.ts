import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { MainFullIdentService } from '../main-full-ident.service';
import { CommentComponent } from './comment/comment.component';

@Component({
  selector: 'app-comments-ident',
  templateUrl: './comments-ident.component.html',
  styleUrls: ['./comments-ident.component.css']
})
export class CommentsIdentComponent implements OnInit {
  @ViewChild('comment', {read: ViewContainerRef}) comment: any;

  interval: any;
  value: any;//обьект с данными передан из родительского компонента main-component
  //по полю advID этого обьекта, из массива всех комментариев выбирается те, которые относятся именно к данному обьявлению
  isComments: boolean = true;

  constructor(private componentFactoryResolver: ComponentFactoryResolver, private mFServ: MainFullIdentService, private appServ: AppService) {
    this.mFServ.getadvChangedInfo().subscribe(() => {
      this.getAds();
    })
   }//экземпляр сервиса
    
    getLoggedStatus(): string{//Проверка роли пользователя
      return this.appServ.getLoggedStatus();
    }

    ngOnInit(): void {}

    addComments(): any{//комментарии выбираются из двумерного массива, где первый индекс это номер обьявления, а второй - сам комментарий
      this.mFServ.getCommentValue(this.value.id).subscribe(data =>{//запрос на получение комменотв
        console.log(data);
        if(data == null){//если комментов нет
          this.isComments = false;//отображается сообщение в интерфейсе
        }else{
          this.isComments = true;
          for(let i = 0; i < data.length; i++){
            let commentItemComponent = this.componentFactoryResolver.resolveComponentFactory(CommentComponent)// берется шаблон компонента
            let commentItemComponentRef = this.comment.createComponent(commentItemComponent);// по шаблону создается элемент
            (<CommentComponent>(commentItemComponentRef.instance)).value = data[i];//добавление данных непосредственно в обьект обьявления
          }
        }
      });
    }

    getAds() {//тайм аут для правилной прогрузки динамических компонентов и добавлении их в шаблон без ошибок
      this.interval = setTimeout(() => {
        this.comment.clear();
        this.addComments();
      }, 0);
    }
}
