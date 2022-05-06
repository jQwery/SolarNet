import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAdvertismentComponent } from './new-advertisment.component';

describe('NewAdvertismentComponent', () => {
  let component: NewAdvertismentComponent;
  let fixture: ComponentFixture<NewAdvertismentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewAdvertismentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAdvertismentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
