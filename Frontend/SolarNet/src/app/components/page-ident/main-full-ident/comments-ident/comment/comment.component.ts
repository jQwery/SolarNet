import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { PageIdentService } from '../../../page-ident.service';
import { MainFullIdentService } from '../../main-full-ident.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {

  value: any;//переменная для хранения данных для каждого комментария

  avatarLink: string = '';

  constructor(private appServ: AppService, private mFServ: MainFullIdentService, private pIServ: PageIdentService) { 
  }

  getLoggedStatus(): string{//проверка роли пользователя
    return this.appServ.getLoggedStatus();
  }

  deleteComment(commentId: any): void{//Удаление коммента
    this.mFServ.deleteComment(commentId).subscribe(data => {
      console.log(data);
      this.mFServ.setadvChangedInfo("new");//При удалении коммента, весь список комментов изменяется и выводится заново
    },(error)=>{console.log(error)})
  }

  getUserName(): boolean{//Получить имя текущего пользователя
    this.pIServ.decodeToken();
    return (this.value.userName == this.pIServ.getUserValues().name);
  }

  ngOnInit(): void {
    console.log(this.getUserName());
  }

  addAvatar(): string{//Добавление аватара для автора обьявления
    if(this.value.avatarLink == 'string' || this.value.avatarLink == null){
      return "https://cs.pikabu.ru/post_img/big/2013/08/24/1/1377296637_1500370441.png"
    }else{
      return this.value.avatarLink;
    }
  }

}
