#include "DraggableItemWidget.h"
#include "ui_DraggableItemWidget.h"

DraggableItemWidget::DraggableItemWidget(QWidget *parent)
	: QWidget(parent, 0), m_ui(new Ui::DraggableItemWidget)
{
	m_ui->setupUi(this);
}

DraggableItemWidget::~DraggableItemWidget()
{

}