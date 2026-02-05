import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-confirmation-message',
  imports: [RouterLink],
  templateUrl: './confirmation-message.html',
  styleUrl: './confirmation-message.css',
})
export class ConfirmationMessage {

  protected message: string = "";
  protected showShoppingLink: boolean = false;

  constructor(private route: ActivatedRoute) {
    const messagecode = this.route.snapshot.paramMap.get('messagecode');
    switch (messagecode) {
      case "1":
        this.message = "Your order has been placed successfully!";
        break;
      case "2":
        this.message = "There was an issue processing your order. Please try again.";
        break;
      default:
        this.message = "Unknown confirmation status.";
        break;
    }

    this.showShoppingLink = messagecode === "1" || messagecode === "2";

  }

}
