import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';
import { MessageService, MessageDto } from '../message.service';

@Component({
  selector: 'app-view-message',
  templateUrl: './view-message.component.html',
  styleUrls: ['./view-message.component.scss']
})
export class ViewMessageComponent implements OnInit, OnDestroy {
  id: string;
  message: MessageDto;

  timer: NodeJS.Timer;
  secondsUntilExpiration = 0.0;

  content = '';
  showContentError = false;
  isMessageInvalid = false;
  isDecryptingMessage = false;
  hasCopiedToClipboard = false;
  messageForm = this.fb.group({
    key: ['', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.messageService.getMessage(this.id).subscribe(
      result => {
        this.secondsUntilExpiration = (new Date(result.expiresAt).getTime() - new Date().getTime()) / 1000;
        if (this.secondsUntilExpiration > 0) {
          this.message = result;
          this.timer = setInterval(() => {
            if (--this.secondsUntilExpiration < 0) {
              this.message = null;
              this.isMessageInvalid = true;
              clearInterval(this.timer);
            }
          }, 1000);
        } else {
          this.isMessageInvalid = true;
        }
      }, err => {
        this.isMessageInvalid = true;
      }
    );
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  copyLink(): void {
    const clipboardArea = document.createElement('textarea');
    clipboardArea.style.position = 'fixed';
    clipboardArea.style.left = '0';
    clipboardArea.style.top = '0';
    clipboardArea.style.opacity = '0';
    clipboardArea.value = window.location.href;
    document.body.appendChild(clipboardArea);
    clipboardArea.focus();
    clipboardArea.select();
    document.execCommand('copy');
    document.body.removeChild(clipboardArea);
    this.hasCopiedToClipboard = true;
  }

  onMessageFormSubmit() {
    if (this.isDecryptingMessage) {
      return;
    }

    this.isDecryptingMessage = true;
    const key = this.messageForm.get('key').value;

    this.messageService.decryptMessage(this.id, key).subscribe(
      result => {
        this.content = result;
        this.showContentError = false;
        this.isDecryptingMessage = false;
      }, err => {
        this.content = '';
        this.showContentError = true;
        this.isDecryptingMessage = false;
        if (new Date(this.message.expiresAt) < new Date()) {
          this.isMessageInvalid = true;
        }
      }
    );
  }
}
