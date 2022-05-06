import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AdvertismentFullIdentComponent } from '../advertisment-full-ident/advertisment-full-ident.component';
import { CommentsIdentComponent } from '../comments-ident/comments-ident.component';
import { MainFullIdentComponent } from '../main-full-ident.component';
import { MainFullIdentService } from '../main-full-ident.service';
import { CommentComponent } from '../comments-ident/comment/comment.component';
import { AppRoutingModule } from 'src/app/app-routing.module';



@NgModule({
  declarations: [
    AdvertismentFullIdentComponent,
    CommentsIdentComponent,
    MainFullIdentComponent,
    CommentComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  exports: [
    MainFullIdentComponent
  ],
  entryComponents:[
    AdvertismentFullIdentComponent,
    CommentsIdentComponent, 
    CommentComponent
  ],
  providers: [MainFullIdentService],
  bootstrap: [MainFullIdentComponent]
})
export class MainFullIdentModule { }
