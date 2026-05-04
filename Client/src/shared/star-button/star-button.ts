import { Component, EventEmitter, input, Input, output, Output } from '@angular/core';

@Component({
  selector: 'app-star-button',
  imports: [],
  templateUrl: './star-button.html',
  styleUrl: './star-button.css',
})
export class StarButton {
  isSelected = input<boolean>();
  disabled = input<boolean>();
  clickEvent = output<Event>();

  // @Input() isSelected: boolean = false;
  // @Input() disabled: boolean = false;
  // @Output() clickEvent = new EventEmitter<void>();  

  onClick(event: Event) {
    this.clickEvent.emit(event);
  }

  // onClick() {
  //   if (!this.disabled) {
  //     this.clickEvent.emit();
  //   }
  // }


}
