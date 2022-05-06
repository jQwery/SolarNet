import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
// import { RouterModule, Routes } from '@angular/router'
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';

//Дочерние модули
// import { PageNdModule} from './components/page-nd/page-nd/page-nd.module';
import { PageIdentModule } from './components/page-ident/page-ident/page-ident.module';
// import { PageAdminModule } from './components/page-admin/page-admin/page-admin.module';
// import { MainNdModuleModule } from '../app/components/main/main-nd-module/main-nd-module.module';
// import { MainFullNdModuleModule } from '../app/components/main-full-nd/main-full-nd-module/main-full-nd-module.module';
// import { AuthorizationModuleModule } from '../app/components/authorization/authorization-module/authorization-module.module'


//Компоненты, которые используются непосредственно в этом модуле
import { AppComponent } from './app.component';
// import { HeaderNdComponent } from './components/header-nd/header-nd.component';
// import { FooterComponent } from './components/footer/footer.component';
// import { AdvertismentNdComponent } from '../app/components/main/advertisment-content-nd/advertisment-nd/advertisment-nd/advertisment-nd.component';
// import { PageNdComponent } from './components/page-nd/page-nd.component';

//Сервис
import { AppService } from './app.service';
// import { MiniImgComponent } from './components/page-ident/user-page/mini-img/mini-img.component';
// import { UserComponent } from './components/page-ident/main-ident/user/user.component';
// import { AdvertismentFullAdminComponent } from './components/page-admin/main-full-admin/advertisment-full-admin/advertisment-full-admin.component';
// import { CommentsAdminComponent } from './components/page-admin/main-full-admin/comments-admin/comments-admin.component';
// import { CommentAdminComponent } from './components/page-admin/main-full-admin/comments-admin/comment-admin/comment-admin.component';
// import { MainIdentComponent } from './components/page-ident/main-ident/main-ident.component';
// import { AdvertismentContentIdentComponent } from './components/page-ident/main-ident/advertisment-content-ident/advertisment-content-ident.component';
// import { AdvertismentIdentComponent } from './components/page-ident/main-ident/advertisment-content-ident/advertisment-ident/advertisment-ident.component';
// import { AsideIdentComponent } from './components/page-ident/main-ident/aside-ident/aside-ident.component';
// const appRoutes: Routes = [
//   {path: 'advertisment-full', component: MainComponent},
//   {path: 'advertisments-list', component: MainNdComponent}
// ]

@NgModule({
  declarations: [
    AppComponent,
    // UserComponent,
    // MainIdentComponent,
    // AdvertismentContentIdentComponent,
    // AdvertismentIdentComponent,
    // AsideIdentComponent,
    // HeaderNdComponent,
    // FooterComponent,
    // PageNdComponent
  ],
  imports: [
    // PageNdModule,
    PageIdentModule,
    // PageAdminModule,
    BrowserModule,
    AppRoutingModule,
    // MainNdModuleModule,
    // MainFullNdModuleModule,
    // AuthorizationModuleModule,
    FormsModule
    // RouterModule.forRoot(appRoutes)
  ],
  providers: [AppService],
  bootstrap: [AppComponent],
  entryComponents:[
    // AdvertismentNdComponent
  ]
})
export class AppModule { }
