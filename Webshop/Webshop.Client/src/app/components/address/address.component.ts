import {Component, EventEmitter, Input, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import {Address} from "../../interfaces/Address";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-address',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './address.component.html',
  styles: [
  ]
})
export class AddressComponent {
  @Input() address!: Address;

  @Output() addressChange = new EventEmitter<Address>();

  constructor() { }

  save() {
    this.addressChange.emit(this.address);
  }
}
