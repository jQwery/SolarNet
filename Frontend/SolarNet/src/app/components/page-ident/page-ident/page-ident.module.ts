import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { FormsModule } from '@angular/forms';
 
// import { MainNdModuleModule } from '../../main/main-nd-module/main-nd-module.module';
import { MainFullIdentModule } from '../main-full-ident/main-full-ident/main-full-ident.module';
import { MainIdentModule } from '../main-ident/main-ident/main-ident.module';
import { AuthorizationModuleModule } from '../../authorization/authorization-module/authorization-module.module';


import { HeaderIdentComponent } from '../header-ident/header-ident.component';
import { PageIdentComponent } from '../page-ident.component';
import { FooterIdentComponent } from '../footer-ident/footer-ident.component';
import { AdvertismentIdentComponent } from '../main-ident/advertisment-content-ident/advertisment-ident/advertisment-ident.component';
import { NewAdvertismentComponent } from '../new-advertisment/new-advertisment.component';
import { UserPageComponent } from '../user-page/user-page.component';
import { PageIdentService } from '../page-ident.service';
import { MiniImgComponent } from '../user-page/mini-img/mini-img.component';

@NgModule({
  declarations: [
    FooterIdentComponent,
    HeaderIdentComponent,
    NewAdvertismentComponent,
    PageIdentComponent,
    UserPageComponent,
    MiniImgComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    MainFullIdentModule,
    MainIdentModule,
    AppRoutingModule,
    FormsModule,
    AuthorizationModuleModule
  ],
  exports: [
    PageIdentComponent
  ],
  entryComponents:[
    AdvertismentIdentComponent,
    MiniImgComponent
  ],
  providers:[PageIdentService],
  bootstrap: [PageIdentComponent]
})
export class PageIdentModule { }
