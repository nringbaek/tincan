import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppMaterialModule } from './app-material.module';

import { AppComponent } from './app.component';
import { CreateMessageComponent } from './create-message/create-message.component';
import { ViewMessageComponent } from './view-message/view-message.component';
import { AutofocusDirective } from './autofocus.directive';
import { TimeLeftPipe } from './time-left.pipe';

const routes = [
  { path: ':id', component: ViewMessageComponent },
  { path: '**', component: CreateMessageComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    CreateMessageComponent,
    ViewMessageComponent,
    AutofocusDirective,
    TimeLeftPipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    AppMaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
