import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ViewChild, ElementRef } from '@angular/core';
import { MessageService } from '../message.service';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-create-message',
  templateUrl: './create-message.component.html',
  styleUrls: ['./create-message.component.scss']
})
export class CreateMessageComponent implements OnInit {
  isCreatingMessage = false;
  messageForm = new FormGroup({
    key: new FormControl('', Validators.required),
    content: new FormControl('', Validators.required)
  });

  @ViewChild('flatpickrInput')
  flatpickrInputRef: ElementRef;
  flatpickrInput: any;

  constructor(
    private router: Router,
    private messageService: MessageService
    ) { }

  ngOnInit() {
    this.flatpickrInput = flatpickr(this.flatpickrInputRef.nativeElement, {
      enableTime: true,
      animate: true,
      time_24hr: true,
      defaultDate: new Date(Date.now() + (1 * 60 * 60 * 1000)),
      altInput: true,
      altFormat: 'j. F Y, H:i',
      dateFormat: 'Z',
      locale: {
        firstDayOfWeek: 1
      }
    });
  }

  onMessageFormSubmit() {
    const key = this.messageForm.get('key').value;
    const content = this.messageForm.get('content').value;
    const expiration = this.flatpickrInput.selectedDates[0];

    this.isCreatingMessage = true;
    this.messageService.createMessage(key, expiration, content).subscribe(
      result => {
        this.router.navigate(['/message', result]);
      }, err => {
        this.isCreatingMessage = false;
      }
    );
  }
}
